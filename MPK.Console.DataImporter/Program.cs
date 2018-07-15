using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess;
using MPK.Connect.DataAccess.Stops;
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
            serviceCollection.AddTransient<IStopService, StopService>();
            serviceCollection.AddTransient<IStopRepository, StopRepository>();
        }

        private static void Main(string[] args)
        {
            // Create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var fileName = Configuration.GetSection("Sources:Stops");
            var stopImporterService = serviceProvider.GetService(typeof(IStopService)) as StopService;
            stopImporterService?.ReadStopsFromFile(fileName.Value);

            // Get backup sources for client
            System.Console.ReadLine();
        }
    }
}