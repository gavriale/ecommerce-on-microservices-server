using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order.API.Entities
{


    [Table("OrderItems")]
    public class OrderItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public ShopOrder ShopOrder { get; set; }
        public int ShopOrderId { get; set; }
    }
}
