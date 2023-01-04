using NServiceBus;

namespace StockMessages
{
    
    public class CheckStock : ICommand
    {
        public int Id { get; set; }
    }
}
