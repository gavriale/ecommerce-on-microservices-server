using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;
using OrderMessages;
using Stock.API.Data;
using System.Linq;

namespace Stock.API.StockEventHandlers
{
    public class OrderPlacedHandler :
        IHandleMessages<OrderPlaced>
    {
        static readonly ILog log = LogManager.GetLogger<OrderPlacedHandler>();

        private readonly DataContext _context;

        public OrderPlacedHandler(DataContext context)
        {
            _context = context;
        }

        public async Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            log.Info($"Stock has received OrderPlaced, ProductId = {message.Product}");

            var stockItem = _context.Stock.SingleOrDefault(s => s.Id == message.Product);

            if (stockItem != null && stockItem.Quantity > 0)
            {
                log.Info($"The product is available in the Stock");
                stockItem.Quantity -= 1;
                _context.SaveChanges();

                var orderAvailable = new OrderAvailable
                {
                    OrderId = message.OrderId,
                    Product = message.Product
                };

                await context.Publish(orderAvailable);
            }

            //else publish OrderNotAvailable

            
        }
    }
}
