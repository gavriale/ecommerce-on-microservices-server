using Microsoft.EntityFrameworkCore;
using Order.API.Entities;


namespace Order.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }
        //Orders is the name of the table
        public DbSet<ShopOrder> Orders { get; set; }
    }
}
