using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NServiceBus.Logging;
using OrderMessages;
using Stock.API.Data;
using System.Threading.Tasks;

namespace Stock.API.StockEventHandlers
{
    public class StockHandler : IHandleMessages<ProductCheckMessage>,
        IHandleMessages<RestoreStock>
    {
        static readonly ILog log = LogManager.GetLogger<StockHandler>();

        private readonly DataContext _context;

        public StockHandler(DataContext context)
        {
            _context = context;
        }

        public async Task Handle(ProductCheckMessage message, IMessageHandlerContext context)
        {
            log.Info($"Stock has received ProductCheckMessage, OrderId = {message.OrderId},Quantity = {message.Quantity},ProductId = {message.Product}," +
                $"UserId = {message.UserId}, Price = {message.Price}");

            var product = await _context.Stock.SingleOrDefaultAsync(x => x.Id == message.Product);

            log.Info($"Stock has received ProductCheckMessage,Product = {product.Id},Stock = {product.Quantity}," +
                $"Quantity = {message.Quantity}");

            bool enoughInStock = false;

            if (product.Quantity >= message.Quantity)
            {
                enoughInStock = true;
                product.Quantity-=message.Quantity;
                await _context.SaveChangesAsync();
            }

            log.Info($"Stock has received ProductCheckMessage,Product = {product.Id},Stock = {product.Quantity}," +
                $"new Quantity = {message.Quantity}");

            await context.Publish(new ProductAvailable() 
            { 
                OrderId = message.OrderId, 
                HasProduct = enoughInStock,
                UserId = message.UserId,
                OrderPrice = message.Price,
                ProductId = message.Product,
                Quantity = message.Quantity       
            });
     
        }

        public async Task Handle(RestoreStock message, IMessageHandlerContext context)
        {
            log.Info($"Stock has received RestoreStock,Product = {message.ProductId},Stock = {message.Quantity}," +
                $"new Quantity = {message.Quantity}");
            var product = await _context.Stock.SingleOrDefaultAsync(x => x.Id == message.ProductId);
            product.Quantity += message.Quantity;
            await _context.SaveChangesAsync();
            log.Info($"Stock Restored,Product = {product.Id},Restored Stock = {product.Quantity},");
        }
    }
}
