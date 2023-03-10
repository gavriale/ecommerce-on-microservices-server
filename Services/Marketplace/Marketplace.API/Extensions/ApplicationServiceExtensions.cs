using Marketplace.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.API.Extensions
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

