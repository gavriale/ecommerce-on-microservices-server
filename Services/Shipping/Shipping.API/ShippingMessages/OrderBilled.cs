using NServiceBus;
using System;

namespace OrderMessages
{
    public class OrderBilled : IEvent
    {
        public Guid OrderId { get; set; }

        public string UserName { get; set; }

        public int Price { get; set; }

        public int Product { get; set; }

    }
}
