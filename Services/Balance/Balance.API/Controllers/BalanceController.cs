using Balance.API.Data;
using Balance.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Balance.API.Controllers
{
    public class BalanceController : BaseApiController
    {
        private readonly DataContext _context;
        public BalanceController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BalanceItem>>> GetBalance()
        {
            return await _context.Balance.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BalanceItem>> GetBalance(int id)
        {
            return await _context.Balance.FindAsync(id);
        }

    }
}
