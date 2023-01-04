using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shipping.API.Controllers;
using Shipping.API.Entities;

namespace Shipping.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }
        public DbSet<ShippingItem> Shipping { get; set; }

    }
}
