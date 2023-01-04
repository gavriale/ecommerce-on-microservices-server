using NServiceBus;

namespace InventoryMessages
{
    public class CheckInventory : ICommand
    {
        public int Id { get; set; }
    }
}
