using AddUserBalanceCommand;
using Balance.API.BalanceMessages;
using Balance.API.Data;
using Balance.API.Entities;
using NServiceBus;
using NServiceBus.Logging;
using OrderMessages;
using System.Threading.Tasks;

namespace Balance.API.BalanceEventHandlers
{
    public class BalanceCheckedHandler : IHandleMessages<BillUser>,
        IHandleMessages<AddUserBalance>, IHandleMessages<OrderPlaced>
    {
        static ILog log = LogManager.GetLogger<BalanceCheckedHandler>();
        private readonly DataContext _context;

        
        public BalanceCheckedHandler(DataContext context)
        {
            _context = context;
        }
        

        public async Task Handle(BillUser message, IMessageHandlerContext context)
        {
            log.Info($"Balance has received OrderPlaced, UserId = {message.UserId} ,Order Price = {message.OrderPrice}");

            BalanceItem userBalance = await _context.Balance.FindAsync(message.UserId);
            var wasBilled = false;

            if(userBalance != null && userBalance.Credit >= message.OrderPrice)
            {
                wasBilled = true;
                userBalance.Credit-=message.OrderPrice;
                log.Info($"WasBilled is true!!!!!!!");

                await _context.SaveChangesAsync();
            }

            var orderBilled = new OrderBilled
            {
                OrderId = message.OrderId,
                Price = message.OrderPrice,
                UserId = message.UserId,
                ProductId = message.ProductId,
                Quantity = message.Quantity,
                WasBilled = wasBilled
            };

            log.Info($"Balance publishing OrderBilled message: Was Billed = {orderBilled.WasBilled}");
            await context.Publish(orderBilled);     
        }

        public async Task Handle(AddUserBalance message, IMessageHandlerContext context)
        {
            log.Info($"Balance has received AddUserBalance command, UserId = {message.UserId}, Balance = {message.Balance}!");
            var balanceForUser = new BalanceItem
            {
                Id = message.UserId,
                Credit = message.Balance,
            };

            _context.Balance.Add(balanceForUser);
            await _context.SaveChangesAsync();

            log.Info($"Balance object added to the Balance table");
            await Task.CompletedTask;
        }

        public async Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            await Task.CompletedTask;
        }
    }
}
