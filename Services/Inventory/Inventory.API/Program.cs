using Inventory.API.Entities;
using InventoryMessages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API
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

                    var endpointConfiguration = new EndpointConfiguration("Inventory");

                    var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();

                    transport.ConnectionString("host=rabbitmq");

                    var routing = transport.UseConventionalRoutingTopology().Routing();

                    /**
                     * The rabbitmq routing defined with the type of message - which is an object
                     * and the destination defined as the endpoint - sent to Inventory
                     * */
                    routing.RouteToEndpoint(messageType: typeof(CheckInventory), destination: "Order");


                    endpointConfiguration.EnableInstallers();

                    return endpointConfiguration;
                    })
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    });



    }
}
