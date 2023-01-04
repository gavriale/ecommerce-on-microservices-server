using Inventory.API.Data;
using Inventory.API.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Inventory.API.Controllers
{
    public class InventoryItemController : BaseApiController
    {

        private readonly DataContext _context;
        public InventoryItemController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryItem>> GetInventoryItem(int id)
        {
            return await _context.Items.FindAsync(id);
        }

    }
}
