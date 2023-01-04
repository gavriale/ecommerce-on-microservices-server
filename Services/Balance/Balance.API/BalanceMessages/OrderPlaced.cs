using NServiceBus;
using System;

namespace OrderMessages
{
    public class OrderPlaced : IEvent
    {
        public int OrderId { get; set; }

        public string UserName { get; set; }

        public int Price { get; set; }

        public int Product { get; set; }

        public string Address { get; set; }

    }
}
