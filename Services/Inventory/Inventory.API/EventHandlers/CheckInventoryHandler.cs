using InventoryMessages;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;

namespace Inventory.API.EventHandlers
{
    public class CheckInventoryHandler : IHandleMessages<CheckInventory>
    {

        static ILog log = LogManager.GetLogger<CheckInventoryHandler>();

        public Task Handle(CheckInventory message, IMessageHandlerContext context)
        {
            log.Info($"Shipping has received OrderPlaced, OrderId = {message.Id}");
            return Task.CompletedTask;
        }
    }
}
