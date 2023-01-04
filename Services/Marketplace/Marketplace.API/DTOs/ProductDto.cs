namespace Marketplace.API.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int UnitsInStock { get; set; }
        public string UnitPrice { get; set; }
    }
}
