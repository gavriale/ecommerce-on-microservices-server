using Marketplace.API.Data;
using Marketplace.API.DTOs;
using Marketplace.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NServiceBus;
using StockMessages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.API.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ILogger<ProductController> _logger;
        private readonly IMessageSession _messageSession;
        public ProductController(DataContext context, ILogger<ProductController> logger,
            IMessageSession messageSession)
        {
            _context = context;
            _logger = logger;
            _messageSession = messageSession;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        [HttpPost("{addproduct}")]
        public async Task<ActionResult<ProductDto>> addProduct(ProductDto product)

        {

            //_logger.LogInformation($"-------------add new product : {product.Id}-{product.Name}-{product.UnitPrice}-------");


            Product newProduct = new Product

            {
                Id = product.Id,

                Name = product.Name,

                UnitPrice = product.UnitPrice,

                Description = product.Description,

                ImageUrl = product.ImageUrl,

                UnitsInStock = product.UnitsInStock
            };

            _logger.LogInformation("------------Sending Message AddToStock to Stock!-------------------");

            await _messageSession.Send(
            new AddToStock
            {
                Id = newProduct.Id,
                Quantity = newProduct.UnitsInStock
            });

            _context.Products.Add(newProduct);

            await _context.SaveChangesAsync();
            return product;

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
             Product p = await _context.Products.FindAsync(id);
             _context.Products.Remove(p);
             await _context.SaveChangesAsync();
             return p;
        }


    }
}
