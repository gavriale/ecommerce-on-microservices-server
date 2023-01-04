using Microsoft.EntityFrameworkCore;
using Stock.API.Entities;

namespace Stock.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }
        public DbSet<StockItem> Stock { get; set; }

    }
}
