using NServiceBus;
using System;

namespace OrderMessages
{
    public class OrderBilled : IEvent
    {
        public int OrderId { get; set; }

        public int UserId { get; set; }

        public int Price { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public bool WasBilled { get; set; }

    }
}
