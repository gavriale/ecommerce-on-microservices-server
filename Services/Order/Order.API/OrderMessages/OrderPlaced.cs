using NServiceBus;
using System;

namespace OrderMessages
{
    public class OrderPlaced : IEvent
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int Price { get; set; }
        public int ProductId { get; set; }
        public int quantity { get; set; }
    }
}
