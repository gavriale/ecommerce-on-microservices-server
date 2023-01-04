using Balance.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Balance.API.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }
        public DbSet<BalanceItem> Balance { get; set; }

    }
}
