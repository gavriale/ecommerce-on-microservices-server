using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock.API.Data;
using Stock.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stock.API.Controllers
{
    
        public class StockController : BaseApiController
        {
            private readonly DataContext _context;
            public StockController(DataContext context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<StockItem>>> GetStock()
            {
                return await _context.Stock.ToListAsync();
            }

            [HttpPost]
            public async Task<ActionResult<StockItem>> GetStock(StockItem item)
            {

            StockItem newItem = new StockItem

            {
                Id = item.Id,
                Quantity = item.Quantity,
            };


            _context.Stock.Add(newItem);

            await _context.SaveChangesAsync();
            return newItem;
        }

    }
}
