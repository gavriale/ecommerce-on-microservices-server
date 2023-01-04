
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NServiceBus;
using NServiceBus.Logging;
using Order.API.Data;
using Order.API.DTOs;
using Order.API.Entities;
using OrderMessages;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Order.API.Controllers
{
    public class OrderDetailsController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ILogger<OrderDetailsController> log;
        private readonly IMessageSession _messageSession;
        private readonly IMapper _mapper;


        public OrderDetailsController(DataContext context,ILogger<OrderDetailsController> logger, IMessageSession messageSession,
            IMapper mapper)
        {
            _context = context;
            log = logger;
            _messageSession = messageSession;
            _mapper = mapper;
        }

    

        public async Task<IEnumerable<UserOrdersDto>> GetOrder()
        {
            var orders = await _context.Orders.Include(x => x.OrderItem).ToListAsync();
            log.LogInformation("------------get order!-------------------");
            var ordersToReturn = _mapper.Map<IEnumerable<UserOrdersDto>>(orders);
            return ordersToReturn;
        }


        [HttpGet("{id}")]
        public async Task<IEnumerable<UserOrdersDto>> GetOrdersByUserId(int id) 
        {

            log.LogInformation("------------return test user!-------------------");
            var res = await _context.Orders
                .Where(o => o.UserId == id).Include(x => x.OrderItem).ToListAsync();

            var ordersToReturn = _mapper.Map<IEnumerable<UserOrdersDto>>(res);

            //var text = JsonConvert.SerializeObject(res, Formatting.None,
            //            new JsonSerializerSettings()
            //            {
            //                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //            });


            //log.LogInformation($"**** {text}");

            //var userOrders = new List<UserOrdersDto>();


            /*
            foreach (var order in res)
            {
                userOrders.Add(new UserOrdersDto
                {
                    Id = order.Id,
                    Price = order.Price,
                    ProductId = order.OrderItem.ProductId,
                    Quantity = order.OrderItem.Quantity,
                    DateOfShipment = order.DateOfShipmment,
                    Status = order.Status,
                });
            }
            */
            
            //var text2 = JsonConvert.SerializeObject(userOrders, Formatting.None,
            //            new JsonSerializerSettings()
            //            {
            //                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //            });

            //log.LogInformation($"**** {text2}");

            return ordersToReturn;    
        }

       

        [HttpPost("checkout")]
        public async Task<ShopOrder> checkout([FromBody] ShopOrderDto shopOrderDto) {

            var orderItem = new OrderItem
            {
                ProductId = shopOrderDto.ProductId,
                Quantity = shopOrderDto.Quantity,
            };

            log.LogInformation($" ShopOrderDto = {shopOrderDto.ProductId},{shopOrderDto.UserId}" +
                $",{shopOrderDto.Quantity}");
  
            log.LogInformation($"orderitem = {orderItem.ProductId},{orderItem.Quantity}");
            log.LogInformation($"DateOfShipping = {shopOrderDto.DateOfShipping}");

            var newOrder = new ShopOrder
            {
                UserId = shopOrderDto.UserId,      
                Price = 0,
                OrderItem = orderItem,
                Status = 1,
                DateOfShipmment = shopOrderDto.DateOfShipping,
            };

            log.LogInformation($"DateOfShipmment of newOrder = {newOrder.DateOfShipmment}");

            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();


            await _messageSession.Send(
            new ComputeOrderPrice
            {
                OrderId = newOrder.Id,
                UserId = shopOrderDto.UserId,
                ProductId = shopOrderDto.ProductId,
                Quantity = shopOrderDto.Quantity,
                Price = 0,
            });

            return newOrder;
        }

    }   
}
