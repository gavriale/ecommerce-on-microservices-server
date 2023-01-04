using NServiceBus;
using NServiceBus.Logging;
using Order.API.Data;
using OrderMessages;
using System.Threading.Tasks;

namespace Order.API.EventHandlers
{
    public class OrderHandler : IHandleMessages<ComputeOrderPrice>
    {

        static ILog log = LogManager.GetLogger<OrderHandler>();
        private readonly DataContext _context;
        
        public OrderHandler(DataContext context)
        {
            _context = context;
        }

        public async Task Handle(ComputeOrderPrice message, IMessageHandlerContext context)
        {
            var order = await _context.Orders.FindAsync(message.OrderId);

            log.Info($"Order has received ComputeOrderPrice, OrderId = {order.Id}, OrderId = {message.OrderId}" +
                $",Price = {message.Price}");

            order.Price = message.Price;

            await _context.SaveChangesAsync();

            var orderPlaced = new OrderPlaced
            {
                OrderId = message.OrderId,
                UserId = message.UserId,
                ProductId = message.ProductId,
                quantity = message.Quantity,
                Price = order.Price
            };

            log.Info("------------publish Order Placed!-------------------");
            log.Info($"OrderId = {orderPlaced.OrderId},UserId = {orderPlaced.UserId} Order Price = {orderPlaced.Price}" +
                $"ProductId = {orderPlaced.ProductId},Quantity = {orderPlaced.quantity}");

            await context.Publish(orderPlaced).ConfigureAwait(false);

        }

       
    }
}
