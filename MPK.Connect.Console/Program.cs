﻿using System;
using System.IO;
using Autofac;
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
using MPK.Connect.Service.Business.HarmonySearch.Generator;
using MPK.Connect.Service.Helpers;
using MPK.Connect.TestEnvironment.Settings;

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
            containerBuilder.RegisterType<ExcelExportService>().AsImplementedInterfaces();
            containerBuilder.RegisterType(typeof(HarmonySearchStopTimeTester)).AsSelf();

            Container = containerBuilder.Build();
        }

        private static void Main(string[] args)
        {
            // Create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            using (var scope = Container.BeginLifetimeScope())
            {
                var automaticTester = scope.Resolve<HarmonySearchStopTimeTester>();

                var scenarios = new HarmonySearchTestScenario(ObjectiveFunctionType.Comprehensive, HarmonyGeneratorType.RandomDirectedStop);

                var source = new Location("Kwiska");
                var destination = new Location("Gajowa");

                automaticTester.RunTestsWithScenarios(scenarios, source, destination);
            }
        }
    }
}