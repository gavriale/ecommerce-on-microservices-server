using NServiceBus;
using System;

namespace OrderMessages
{
    public class OrderAvailable : IEvent
    {
        public Guid OrderId { get; set; }

        public int Product { get; set; }


    }
}
