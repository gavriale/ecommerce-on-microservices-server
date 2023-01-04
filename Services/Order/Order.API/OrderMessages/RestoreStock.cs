using NServiceBus;

namespace OrderMessages
{
    public class RestoreStock : ICommand
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
