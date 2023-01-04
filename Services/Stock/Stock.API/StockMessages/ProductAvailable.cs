using NServiceBus;
using System;

namespace OrderMessages
{
    public class ProductAvailable : IEvent
    {
        public int OrderId { get; set; }
        public bool HasProduct { get; set; }
        public int UserId { get; set; }
        public int OrderPrice { get; set; }

        public int ProductId { get; set; }
        public int Quantity { get; set; }

    }
}
