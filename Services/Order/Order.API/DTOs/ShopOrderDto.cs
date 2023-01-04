using Order.API.Entities;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Order.API.DTOs
{
    public class ShopOrderDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime DateOfShipping { get; set; }
    }
}
