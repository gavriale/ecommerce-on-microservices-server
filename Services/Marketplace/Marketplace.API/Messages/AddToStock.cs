using NServiceBus;

namespace StockMessages
{
    public class AddToStock : ICommand
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

    }
}

