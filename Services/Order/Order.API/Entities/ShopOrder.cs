using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Order.API.Entities
{
    public class ShopOrder
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Price { get; set; }
        public DateTime DateOfShipmment { get; set; }
        public OrderItem OrderItem { get; set; }
        public int Status { get; set; }
    }
}
