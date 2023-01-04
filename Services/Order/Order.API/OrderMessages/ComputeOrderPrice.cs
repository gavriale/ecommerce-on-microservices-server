using NServiceBus;

namespace OrderMessages
{
    public class ComputeOrderPrice :ICommand
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }

    }
}
