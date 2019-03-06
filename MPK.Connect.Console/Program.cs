using System;
using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Service.Business;
using MPK.Connect.Service.Business.Graph;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Experiment;
using MPK.Connect.Service.Export;

namespace MPK.Connect.Console
{
    internal class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static IContainer Container { get; set; }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Add logging
            services.AddSingleton(new LoggerFactory())
                .AddLogging(configure =>
                    configure
                        .AddConsole()
                        .AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning))
                .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);

            // Build configuration
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            // Add access to generic IConfigurationRoot
            services.AddSingleton(Configuration);

            // Add dbContext
            services
                .AddDbContext<SimpleMpkContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString(nameof(SimpleMpkContext))));

            // Add services
            services.AddTransient(typeof(IGenericRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IStopService, StopService>();
            services.AddTransient<ITimeTableService, TimeTableService>();
            services.AddTransient<IStopTimeService, StopTimeService>();
            services.AddTransient<ITravelPlanService, TravelPlanService>();
            services.AddTransient<IGraphBuilder, GraphBuilder>();
            services.AddTransient<IStopPathFinder, StopPathFinder>();
            services.AddTransient<IPathProvider, PathProvider>();
            services.AddTransient<ICoordinateLimitsProvider, CoordinateLimitsProvider>();
            services.AddTransient(typeof(IGenericRepository<>), typeof(BaseRepository<>));

            // Configure Autofac
            var containerBuilder = new ContainerBuilder();

            containerBuilder.Populate(services);
            containerBuilder.RegisterType(typeof(SimpleMpkContext)).As<IMpkContext>();
            containerBuilder.RegisterType<ActionTimer>().AsImplementedInterfaces();
            containerBuilder.RegisterType<ExporterService>().AsImplementedInterfaces();
            containerBuilder.RegisterGeneric(typeof(HarmonySearchAutomaticTester<>)).AsSelf();

            Container = containerBuilder.Build();
        }

        private static void Main(string[] args)
        {
            // Create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            using (var scope = Container.BeginLifetimeScope())
            {
                var graphBuilder = scope.Resolve<IGraphBuilder>();

                var source = new Location("Kwiska");
                var destination = new Location("Świdnicka");

                //var graph = graphBuilder.GetGraph(DateTime.Now);
                //var automaticTester = scope.Resolve<HarmonySearchAutomaticTester<StopTimeInfo>>();

                //var graphObjectiveFunction = new GraphObjectiveFunction(graph, source, destination);
                //var harmonySearcher = new GeneralHarmonySearcher<StopTimeInfo>(graphObjectiveFunction, 20, 5000);

                //automaticTester.RunTests(harmonySearcher, source, destination);

                var automaticTester = scope.Resolve<HarmonySearchAutomaticTester<StopDto>>();

                var stopGraph = graphBuilder.GetStopGraph(DateTime.Now);

                var graphObjectiveFunction = new StopGraphObjectiveFunction(stopGraph, source, destination);
                var harmonySearcher = new GeneralHarmonySearcher<StopDto>(graphObjectiveFunction, 20, 5000);

                automaticTester.RunTests(harmonySearcher, source, destination);
            }
        }
    }
}