﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Service.Business;
using MPK.Connect.Service.Business.Graph;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Helpers;
using MPK.Connect.TestEnvironment.Settings;
using System;
using System.Collections.Generic;
using System.IO;

namespace MPK.Connect.TestEnvironment
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
            services.AddDbContext<SimpleMpkContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString(nameof(SimpleMpkContext))));

            // Add services
            services.AddTransient(typeof(IGenericRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IStopService, StopService>();
            services.AddTransient<ITimeTableService, TimeTableService>();
            services.AddTransient<IStopTimeService, StopTimeService>();
            services.AddTransient<ITravelPlanService, TravelPlanService>();
            services.AddTransient<IGraphBuilder, GraphBuilder>();
            services.AddTransient<IStopTimePathFinder, StopTimePathFinder>();
            services.AddTransient<IPathProvider, PathProvider>();
            services.AddTransient<ICoordinateLimitsProvider, CoordinateLimitsProvider>();
            services.AddTransient(typeof(IGenericRepository<>), typeof(BaseRepository<>));

            // Configure Autofac
            var containerBuilder = new ContainerBuilder();

            containerBuilder.Populate(services);
            containerBuilder.RegisterType<SimpleMpkContext>().As<IMpkContext>();
            containerBuilder.RegisterType<ActionTimer>().AsImplementedInterfaces();
            containerBuilder.RegisterType<ExcelExportService>().AsImplementedInterfaces();
            containerBuilder.RegisterType<HarmonySearchTester>().AsSelf();

            Container = containerBuilder.Build();
        }

        private static void Main(string[] args)
        {
            // Create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            using (var scope = Container.BeginLifetimeScope())
            {
                var tester = scope.Resolve<HarmonySearchTester>();

                var scenarios = new HarmonySearchTestScenario(ObjectiveFunctionType.Comprehensive);

                RunTestWithMultipleRoutes(tester, scenarios);
            }
        }

        private static void RunTestWithMultipleRoutes(HarmonySearchTester tester, HarmonySearchTestScenario scenarios)
        {
            var testRoutes = new List<Tuple<Location, Location>>
            {
                new Tuple<Location, Location>(new Location("Grunwaldzka"), new Location("Ogród Botaniczny")),
                //new Tuple<Location, Location>(new Location("Rynek"), new Location("Pl. Grunwaldzki")),
                //new Tuple<Location, Location>(new Location("Bajana"), new Location("FAT")),
               // new Tuple<Location, Location>(new Location("Biskupin"), new Location("Port Lotniczy"))
            };

            tester.RunTestsWithLocations(testRoutes, scenarios, new DateTime(2019, 05, 13, 16, 0, 0));
        }
    }
}