﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess;
using MPK.Connect.DataAccess.Agencies;
using MPK.Connect.DataAccess.Stops;
using MPK.Connect.Model;
using MPK.Connect.Service;
using System;
using System.IO;
using System.Linq;

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
                .AddLogging(configure =>
                    configure.AddConsole()
                        .AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning))
                .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Warning);

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
            serviceCollection.AddTransient<IImporterService<Stop>, StopService>();
            serviceCollection.AddTransient<IStopRepository, StopRepository>();
            serviceCollection.AddTransient<IImporterService<Agency>, AgencyService>();
            serviceCollection.AddTransient<IAgencyRepository, AgencyRepository>();
        }

        private static void Main(string[] args)
        {
            // Create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var fileNames = Configuration.GetSection("GTFS").GetChildren().Select(a => new { a.Key, a.Value }).ToList();

            //var service = serviceProvider.GetService(typeof(IImporterService<Agency>)) as IImporterService<Agency>;
            //service?.ImportEntitiesFromFile(fileName.Value);

            // Get backup sources for client
            System.Console.ReadLine();
        }
    }
}