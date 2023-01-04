using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shipping.API.Data;
using Shipping.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shipping.API.Controllers
{
    public class ShippingController : BaseApiController
    {
        private readonly DataContext _context;
        public ShippingController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShippingItem>>> GetShipping()
        {
            return await _context.Shipping.ToListAsync();
        }

    }
}
