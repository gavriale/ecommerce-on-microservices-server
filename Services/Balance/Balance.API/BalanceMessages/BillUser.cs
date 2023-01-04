using NServiceBus;

namespace OrderMessages
{
    public class BillUser : ICommand
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }

        public int OrderPrice { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
