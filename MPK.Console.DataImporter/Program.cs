using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess;
using MPK.Connect.DataAccess.Agencies;
using MPK.Connect.DataAccess.Routes.Types;
using MPK.Connect.DataAccess.Stops;
using MPK.Connect.Model;
using MPK.Connect.Service;

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
                .AddLogging(configure => configure.AddConsole().AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning))
                .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information)
                .AddTransient<StopRepository>()
                .AddTransient<StopService>();

            // Build configuration
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            // Add access to generic IConfigurationRoot
            serviceCollection.AddSingleton(Configuration);

            // Add dbContext
            serviceCollection.AddDbContext<MpkContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MpkContext")));

            // Add services
            serviceCollection.AddTransient<IGenericService<Stop>, StopService>();
            serviceCollection.AddTransient<IStopRepository, StopRepository>();
            serviceCollection.AddTransient<IGenericService<RouteType>, RouteTypeService>();
            serviceCollection.AddTransient<IRouteTypeRepository, RouteTypeRepository>();
            serviceCollection.AddTransient<IGenericService<Agency>, AgencyService>();
            serviceCollection.AddTransient<IAgencyRepository, AgencyRepository>();
        }

        private static void Main(string[] args)
        {
            // Create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            //var fileName = Configuration.GetSection("Sources:Stops");
            //var stopImporterService = serviceProvider.GetService(typeof(IGenericService<Stop>)) as IGenericService<Stop>;
            //stopImporterService?.ReadFromFile(fileName.Value);

            //var fileName = Configuration.GetSection("Sources:RouteTypes");
            //var stopImporterService = serviceProvider.GetService(typeof(IGenericService<RouteType>)) as IGenericService<RouteType>;
            //stopImporterService?.ReadFromFile(fileName.Value);

            var fileName = Configuration.GetSection("Sources:Agencies");
            var service = serviceProvider.GetService(typeof(IGenericService<Agency>)) as IGenericService<Agency>;
            service?.ReadFromFile(fileName.Value);

            // Get backup sources for client
            System.Console.ReadLine();
        }
    }
}