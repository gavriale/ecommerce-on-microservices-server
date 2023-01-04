using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NServiceBus.Logging;
using Stock.API.Data;
using Stock.API.Entities;
using StockMessages;
using System.Threading.Tasks;

namespace Stock.API.StockEventHandlers
{
    public class CheckStockHandler : IHandleMessages<CheckStock>,
        IHandleMessages<AddToStock>

    {
        static ILog log = LogManager.GetLogger<CheckStockHandler>();
        private readonly DataContext _context;

        public CheckStockHandler(DataContext context)
        {
            _context = context;
        }

        public Task Handle(CheckStock message, IMessageHandlerContext context)
        {
            log.Info($"Stock has received CheckOrder, OrderId = {message.Id}");
            return Task.CompletedTask;
        }


        public async Task Handle(AddToStock message, IMessageHandlerContext context)
        {

            var product = await _context.Stock.SingleOrDefaultAsync(x => x.Id == message.Id);

            log.Info($"Stock has received AddToStock command, ProductId = {message.Id}, Balance = {message.Quantity}!");
            var ProductToStock = new StockItem
            {
                Id = message.Id,
                Quantity = message.Quantity,
            };

            _context.Stock.Add(ProductToStock);
            await _context.SaveChangesAsync();

            log.Info($"Stock object added to the Stock table");
            await Task.CompletedTask;
        }

    }
}
