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
using MPK.Connect.Service.Helpers;

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
                        .AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Information))
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
                var random = new Random();
                for (var i = 0; i < 10; i++)
                {
                    var startTime = DateTime.Now.TimeOfDay;
                    var randomwInd = random.Next(59);
                    startTime = new TimeSpan(startTime.Hours, randomwInd, startTime.Seconds);
                    var endTime = startTime + new TimeSpan(0, 90, 0);
                    var start = DateTime.Now;
                    var stoptimes = stopTimeRepo.GetAll()
                            .Where(st => st.Trip.ServiceId == "6")
                        .Where(st => startTime < st.DepartureTime && st.DepartureTime < endTime)
                        .ToList();
                    var stop = DateTime.Now;
                    Console.WriteLine($"Retrieved {stoptimes.Count} entities.");
                    Console.WriteLine($"Elapsed time: {stop - start}.");
                }

                var stopRepo = scope.Resolve<IGenericRepository<Stop>>();
                var calendarRepo = scope.Resolve<IGenericRepository<Calendar>>();
                var graphBuilder = new GraphBuilder(stopRepo, stopTimeRepo, calendarRepo);

                var graph = graphBuilder.GetGraph(DateTime.Now.Date);

                var srcs = graph.Nodes.Values
                    .Where(s => s.Data.StopDto.Name.TrimToLower() == "Galeria Dominikańska".TrimToLower())
                    .GroupBy(s => s.Data.StopDto)
                    .ToDictionary(k => k.Key, g => g.Select(s => s.Id).ToList());

                var end = graph.Nodes.Values.First(s => s.Data.StopDto.Name.TrimToLower() == "Biskupin".TrimToLower()).Data.StopDto;

                var distanceFromStopToDestination = srcs.First().Key.GetDistanceTo(end);
                var distancesToStops = new Dictionary<string, double>();
                foreach (var stop in srcs)
                {
                    foreach (var stoptimeid in stop.Value)
                    {
                        var neighborStops = graph.GetNeighbors(stoptimeid)
                            .Select(n => n.Data.StopDto)
                            .Where(n => n.Name.TrimToLower() != "Galeria Dominikańska".TrimToLower())
                            .ToList();

                        foreach (var neighborStop in neighborStops)
                        {
                            var distanceToDestination = end.GetDistanceTo(neighborStop);
                            if (distancesToStops.ContainsKey(stop.Key.Id) && distanceToDestination < distancesToStops[stop.Key.Id])
                            {
                                distancesToStops[stop.Key.Id] = distanceToDestination;
                            }
                            else
                            {
                                distancesToStops[stop.Key.Id] = distanceToDestination;
                            }
                        }
                    }
                }

                var closestStopsKeys = distancesToStops.Where(s => s.Value < distanceFromStopToDestination).OrderBy(s => s.Value).Select(k => k.Key).ToList();

                var filteredSrcs = graph.Nodes.Values
                    .Where(s => closestStopsKeys.Contains(s.Data.StopId))
                    .GroupBy(s => s.Data.StopId)
                    .SelectMany(g => g.OrderBy(st => st.Data.DepartureTime).Take(2))
                    // .SelectMany(g => g.GroupBy(st => st.Data.Route).Select(gr => gr.OrderBy(st => st.Data.DepartureTime).First()))
                    .ToList();

                var probablepaths = new List<Path<StopTimeInfo>>();
                foreach (var src in filteredSrcs)
                {
                    var path = graph.AStar(src.Data, "Biskupin");
                    if (path.Any())
                    {
                        probablepaths.Add(path);
                    }
                }

                probablepaths = probablepaths.OrderBy(p => p.First().DepartureTime).ThenBy(p => p.Cost).ToList();
            }

            Console.WriteLine("Hello World!");
        }
    }
}