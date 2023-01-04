using Login.API.Data;
using Login.API.Interfaces;
using Login.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Login.API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ITokenService, TokenService>();

           
            string mysqlConnectionStr = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<DataContext>(options => {
                options.UseMySql(mysqlConnectionStr, ServerVersion.AutoDetect(mysqlConnectionStr));
            });

            return services;
        }
    }
}
