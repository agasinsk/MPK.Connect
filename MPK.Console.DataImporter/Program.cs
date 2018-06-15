using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess;
using MPK.Connect.Service.Service.Routes;
using MPK.Connect.Service.Service.Stops;

namespace MPK.Console.DataImporter
{
    public class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Add logging
            serviceCollection.AddSingleton(new LoggerFactory())
                .AddLogging(configure => configure.AddConsole())
                .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information)
                .AddTransient<StopsRepository>()
                .AddTransient<StopsService>();

            // Build configuration
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            // Add access to generic IConfigurationRoot
            serviceCollection.AddSingleton(Configuration);

            // Add services
            serviceCollection.AddTransient<IStopsService, StopsService>();
            serviceCollection.AddTransient<IStopsRepository, StopsRepository>();
            serviceCollection.AddTransient<IRouteService, RouteService>();
        }

        private static void Main(string[] args)
        {
            // Create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            //var fileName = Configuration.GetSection("Sources:Csv");
            //var stopImporterService = serviceProvider.GetService(typeof(IStopsService)) as StopsService;
            //stopImporterService?.ReadStopsFromFile(fileName.Value);
            var fileName = Configuration.GetSection("Sources:Routes");
            var routeService = serviceProvider.GetService(typeof(IRouteService)) as RouteService;
            routeService?.ReadRoutesFromFile(fileName.Value);

            // Get backup sources for client
            System.Console.ReadLine();
        }
    }
}