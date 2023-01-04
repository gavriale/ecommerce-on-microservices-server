
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Order.API.Entities;
using OrderMessages;



using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureLogging((hostingContext, loggingBuilder) =>
                {
                    //check log under application settings in the logging section.
                    loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    loggingBuilder.AddConsole();
                    loggingBuilder.AddDebug();
                })
            .UseNServiceBus(context =>
                {

                    var endpointConfiguration = new EndpointConfiguration("Orders");

                    endpointConfiguration.UsePersistence<LearningPersistence>();

                    var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();

                    transport.ConnectionString("host=rabbitmq");

                    var routing = transport.UseConventionalRoutingTopology().Routing();

                    /**
                     * The rabbitmq routing defined with the type of message - which is an object
                     * and the destination defined as the endpoint - sent to Inventory
                     * */
                    routing.RouteToEndpoint(messageType: typeof(ComputeOrderPrice), destination: "Marketplace");
                    routing.RouteToEndpoint(messageType: typeof(ProductCheckMessage), destination: "Stock");
                    routing.RouteToEndpoint(messageType: typeof(BillUser), destination: "Balance");
                    routing.RouteToEndpoint(messageType: typeof(RestoreStock), destination: "Stock");

                    endpointConfiguration.EnableInstallers();

                    return endpointConfiguration;
                });

    }
}
