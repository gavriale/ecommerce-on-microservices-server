using NServiceBus;
using NServiceBus.Logging;
using Order.API.Data;
using OrderMessages;
using System;
using System.Threading.Tasks;

namespace Order.API.EventHandlers
{
    public class OrderPolicy : Saga<OrderPolicy.SagaData>,
        IAmStartedByMessages<OrderPlaced>,
        IAmStartedByMessages<OrderBilled>,
        IAmStartedByMessages<ProductAvailable>,
        IAmStartedByMessages<OrderCancelled>
    {

        static readonly ILog log = LogManager.GetLogger<OrderPolicy>();
        private readonly DataContext _context;

        public OrderPolicy(DataContext context)
        {
            _context = context;
        }

        public async Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            log.Info($"order placed, OrderId = {message.OrderId} - Sending message to Stock.API....");
            log.Info($"order placed, Price = {message.Price} - Sending message to Stock.API....");
            Data.IsOrderPlaced = true;

            await context.Send(new ProductCheckMessage() 
            { 
                OrderId = message.OrderId,
                Product = message.ProductId,
                Quantity = message.quantity,
                UserId = message.UserId,
                Price = message.Price,
                
            });

            await ProcessOrder(context).ConfigureAwait(false);
        }

        public async Task Handle(ProductAvailable message, IMessageHandlerContext context)
        {
            log.Info($"ProductAvailable, UserId = {message.UserId} - OrderPrice = {message.OrderPrice} - OrderId = {message.OrderId}....");
            log.Info($"ProductAvailable, ProductId = {message.ProductId} - Quantity = {message.Quantity}....");

            Data.IsProductAvailable = message.HasProduct;
            if (!message.HasProduct)
            {
  
                var order = await _context.Orders.FindAsync(message.OrderId);
                log.Info($"Changing Order status from the ProductAvailable handle, OrderId = {order.Id}, Status = {order.Status}");
                order.Status = 0;
                await _context.SaveChangesAsync();
                log.Info($"Order status = {order.Id}, Status = {order.Status}");    
                MarkAsComplete();
            }
            log.Info($"ProductAvailable, HasProduct = {message.HasProduct} - Sending message to Balance.API....");

            await context.Send(new BillUser()
            {
                OrderId = message.OrderId,
                ProductId = message.ProductId,
                Quantity = message.Quantity,
                UserId = message.UserId,
                OrderPrice = message.OrderPrice,
            });

            await ProcessOrder(context).ConfigureAwait(false);
        }

        public async Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            log.Info($"OrderBilled, UserId = {message.UserId} - OrderId = {message.OrderId} - OrderId = {message.OrderId}....");
            log.Info($"ProductAvailable, ProductId = {message.ProductId} - Quantity = {message.Quantity}....");

            Data.IsOrderBilled = message.WasBilled;
          
            if (!message.WasBilled)
            {
                //  TODO: Dont repeat youreself
                var order = await _context.Orders.FindAsync(message.OrderId);
                log.Info($"Changing Order status from the ProductAvailable handle, OrderId = {order.Id}, Status = {order.Status}");
                order.Status = 0;
                await _context.SaveChangesAsync();
                log.Info($"Order status = {order.Id}, Status = {order.Status}");

                //  Rollback the stock
                await context.Send(new RestoreStock()
                {          
                    ProductId = message.ProductId,
                    Quantity = message.Quantity,       
                });

                //  MarkAsComplete();
                MarkAsComplete();
            }
            
            await ProcessOrder(context).ConfigureAwait(false);
        }

        public Task Handle(OrderCancelled message, IMessageHandlerContext context)
        {
            throw new System.NotImplementedException();
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
            mapper.MapSaga(SagaData => SagaData.OrderId)
               .ToMessage<OrderPlaced>(message => message.OrderId)
               .ToMessage<OrderBilled>(message => message.OrderId)
               .ToMessage<ProductAvailable>(message => message.OrderId)
               .ToMessage<OrderCancelled>(message => message.OrderId);
        }

        private async Task ProcessOrder(IMessageHandlerContext context)
        {
            if (Data.IsOrderPlaced && Data.IsProductAvailable && Data.IsOrderBilled)
            {
                DateTime today = DateTime.Now;
                DateTime dateToShip = today.AddDays(2);
                
                log.Info($"Order is placed,billed and available, is it time to ship?");
                //if(dateToShip == today){
                //  send message to shipment
                //}
                MarkAsComplete();
            }
        }


        public class SagaData : ContainSagaData
        {
            public int OrderId { get; set; }
            public bool IsOrderPlaced { get; set; }
            public bool IsOrderBilled { get; set; }
            public bool IsProductAvailable { get; set; }
        }

    }
}
