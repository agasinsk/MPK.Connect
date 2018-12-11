﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess;
using MPK.Connect.Model;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Business.Graph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MPK.Connect.Graph
{
    internal class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static IContainer Container { get; set; }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Add logging
            serviceCollection.AddSingleton(new LoggerFactory())
                .AddLogging(configure =>
                    configure
                        .AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning))
                .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);

            // Build configuration
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            // Add access to generic IConfigurationRoot
            serviceCollection.AddSingleton(Configuration);

            // Add dbContext
            serviceCollection.AddDbContext<MpkContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString(nameof(MpkContext))))
                .AddDbContext<SimpleMpkContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString(nameof(SimpleMpkContext))));

            // Add services
            serviceCollection.AddTransient(typeof(IGenericRepository<>), typeof(BaseRepository<>));

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(serviceCollection);

            containerBuilder.RegisterType(typeof(SimpleMpkContext)).As<IMpkContext>();
            Container = containerBuilder.Build();
        }

        private static void Main(string[] args)
        {
            // Create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            using (var scope = Container.BeginLifetimeScope())
            {
                var stopTimeRepo = scope.Resolve<IGenericRepository<StopTime>>();
                var stopRepo = scope.Resolve<IGenericRepository<Stop>>();
                var calendarRepo = scope.Resolve<IGenericRepository<Calendar>>();
                var graphMana = new GraphManager(stopRepo, stopTimeRepo, calendarRepo);
                var graphBounds = new StopMapBounds(51.112457, 17.025346, 51.105209, 17.033606);
                var graph = graphMana.GetGraph(graphBounds);
                var source = graph.Nodes
                    .Where(s => s.Value.Data.Stop.Name == "Rynek")
                    .OrderBy(s => s.Value.Data.DepartureTime).First().Value;
                var destinations = graph.Nodes
                    .Where(s => s.Value.Data.Stop.Name == "Narodowe Forum Muzyki" && s.Value.Data.DepartureTime > source.Data.DepartureTime)
                    .OrderBy(s => s.Value.Data.DepartureTime)
                    .Select(s => s.Value)
                    .ToList();

                var probablePath = graph.AStar(source.Data, "Narodowe Forum Muzyki");

                var paths = new List<Path<StopTimeInfo>>();
                foreach (var destination in destinations)
                {
                    var path = graph.A_Star(source.Data, destination.Data);
                    paths.Add(path);
                }

                paths = paths.Where(p => p.Any()).ToList();
            }

            Console.WriteLine("Hello World!");
        }
    }
}