using NServiceBus;
using System;

namespace OrderMessages
{
    public class OrderAvailable : IEvent
    {
        public int OrderId { get; set; }
        
        public int Product { get; set; }

        
    }
}
