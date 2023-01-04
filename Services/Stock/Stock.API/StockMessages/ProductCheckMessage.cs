using NServiceBus;
using System;

namespace OrderMessages
{
    public class ProductCheckMessage : ICommand
    {
        public int OrderId { get; set; }
        public int Product { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int UserId { get; set; }


    }
}
