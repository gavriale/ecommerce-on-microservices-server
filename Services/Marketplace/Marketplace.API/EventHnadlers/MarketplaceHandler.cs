using Marketplace.API.Data;
using Marketplace.API.Entities;
using NServiceBus;
using NServiceBus.Logging;
using OrderMessages;
using System;
using System.Threading.Tasks;

namespace Marketplace.API.EventBus
{
    public class MarketplaceHandler : IHandleMessages<ComputeOrderPrice>
    {

        static ILog log = LogManager.GetLogger<MarketplaceHandler>();
        private readonly DataContext _context;
        

        public MarketplaceHandler(DataContext context)
        {
            _context = context;
            
        }

        public async Task Handle(ComputeOrderPrice message, IMessageHandlerContext context)
        {

            Product p = await _context.Products.FindAsync(message.ProductId);
            log.Info($"Marketplace has received ComputeOrderPrice, ProductId = {message.ProductId},Quantity = {message.Quantity}," +
                $"OrderId = {message.OrderId}");
            log.Info($"Product details from db ProductId = {p.Id},Quantity = {p.UnitPrice}," +
                $"OrderId = {message.OrderId}");
            var price = Int32.Parse(p.UnitPrice)*message.Quantity;
            log.Info($"Marketplace has received ComputeOrderPrice, Order Price = {price}");

            await context.Send(
            new ComputeOrderPrice
            {
                OrderId = message.OrderId,
                UserId = message.UserId,
                ProductId = p.Id,
                Quantity = message.Quantity,
                Price = price,
            });
        }
    }
}
