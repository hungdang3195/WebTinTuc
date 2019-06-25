using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShopOnlineApp.Data.EF;

namespace ShopOnlineApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host =  BuildWebHost(args);
            using(var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var dbInitializer = services.GetService<DbInitializer>();
                    dbInitializer.Seed().Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database");
                }
            }
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(Configuration)
                .UseContentRoot(Directory.GetCurrentDirectory())   
                .UseStartup<Startup>()
                .Build();

        static void Configuration(WebHostBuilderContext context, IConfigurationBuilder config)
        {
            config.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.jon", optional: true,
                    reloadOnChange: true);
        }

    }
}
