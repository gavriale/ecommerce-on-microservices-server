using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shipping.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .UseNServiceBus(context =>
                 {

                     var endpointConfiguration = new EndpointConfiguration("Shipping");

                     endpointConfiguration.UsePersistence<LearningPersistence>();

                     var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();

                     transport.ConnectionString("host=rabbitmq");

                     var routing = transport.UseConventionalRoutingTopology().Routing();

                     endpointConfiguration.EnableInstallers();

                     return endpointConfiguration;
                 })
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    });
    }
}
