using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
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
                var graphBuilder = new GraphBuilder(stopRepo, stopTimeRepo, calendarRepo);
                var graphBounds = new CoordinateLimits(51.112457, 16.97820, 51.09294, 17.040524);
                var graph = graphBuilder.GetGraph();
                var source = graph.Nodes
                    .Where(s => s.Value.Data.Stop.Name == "FAT")
                    .OrderBy(s => s.Value.Data.DepartureTime).First().Value;

                var sources = graph.Nodes.Values
                    .Where(s => s.Data.Stop.Name.Trim().ToLower() == "FAT".Trim().ToLower())
                    .GroupBy(s => s.Data.Route)
                    .ToDictionary(k => k.Key, v => v.OrderBy(st => st.Data.DepartureTime).First());

                var destinations = graph.Nodes.Values
                    .Where(s => s.Data.Stop.Name == "Galeria Dominikańska" && s.Data.DepartureTime > source.Data.DepartureTime)
                    .OrderBy(s => s.Data.DepartureTime)
                    .GroupBy(s => s.Data.Route)
                    .ToDictionary(k => k.Key, v => v.OrderBy(st => st.Data.DepartureTime).First());

                var probablepaths = new List<Path<StopTimeInfo>>();
                foreach (var src in sources.Values)
                {
                    var path = graph.AStar(src.Data, "Galeria Dominikańska");
                    if (path.Any())
                    {
                        probablepaths.Add(path);
                    }
                }

                var probablePath = graph.AStar(source.Data, "Kwiska");

                var paths = new List<Path<StopTimeInfo>>();
                foreach (var src in sources.Values)
                {
                    foreach (var destination in destinations.Values)
                    {
                        var path = graph.A_Star(src.Data, destination.Data);
                        if (path.Any())
                        {
                            paths.Add(path);
                        }
                    }
                }

                paths = paths.Where(p => p.Any()).OrderBy(p => p.Cost).ToList();
            }

            Console.WriteLine("Hello World!");
        }
    }
}