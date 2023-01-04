using System;

namespace Shipping.API.Entities
{
    public class ShippingItem
    {

        public int Id { get; set; }
        public string UserId { get; set; }

        public Guid ShippmentNumber { get; set; }
        

    }
}
