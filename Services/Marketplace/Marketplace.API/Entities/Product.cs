using System.ComponentModel.DataAnnotations;

namespace Marketplace.API.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int UnitsInStock { get; set; }
        public string UnitPrice { get; set; }

    }
}
