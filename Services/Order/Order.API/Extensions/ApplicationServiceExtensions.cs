using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.API.Data;

namespace Order.API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            
            string mysqlConnectionStr = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<DataContext>(options => {
                options.UseMySql(mysqlConnectionStr, ServerVersion.AutoDetect(mysqlConnectionStr));
            });

            return services;
        }
    }
}
