using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using OrderMessages;
using StockMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.API
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

                     var endpointConfiguration = new EndpointConfiguration("Marketplace");

                     var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();

                     transport.ConnectionString("host=rabbitmq");

                     var routing = transport.UseConventionalRoutingTopology().Routing();

                     /**
                      * The rabbitmq routing defined with the type of message - which is an object
                      * and the destination defined as the endpoint - sent to Inventory
                      * */
                     routing.RouteToEndpoint(messageType: typeof(AddToStock), destination: "Stock");
                     routing.RouteToEndpoint(messageType: typeof(ComputeOrderPrice), destination: "Orders");


                     endpointConfiguration.EnableInstallers();

                     return endpointConfiguration;
                 })
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    });
    }
}
