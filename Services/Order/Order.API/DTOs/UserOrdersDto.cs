using Order.API.Entities;
using System;
using System.Collections.Generic;

namespace Order.API.DTOs
{
    public class UserOrdersDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Price { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime DateOfShipmment { get; set; }
        public int Status { get; set; }
    }
}
