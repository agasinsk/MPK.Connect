using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess;
using MPK.Connect.Model;
using MPK.Connect.Model.Helpers;
using MPK.Connect.Service;
using MPK.Connect.Service.Builders;
using MPK.Connect.Service.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using IEntityImporter = MPK.Connect.Service.IEntityImporter;

namespace MPK.Console.DataImporter
{
    public class Program
    {
        public static IContainer Container { get; set; }
        public static IConfigurationRoot Configuration { get; set; }

        public static IEnumerable<Type> GetAllTypes(Type genericType)
        {
            var platform = Environment.OSVersion.Platform.ToString();
            var runtimeAssemblyNames = DependencyContext.Default.GetRuntimeAssemblyNames(platform);

            return runtimeAssemblyNames
                .Select(Assembly.Load)
                .SelectMany(assembly => assembly.GetTypes()
                    .Where(type =>
                        type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericType) &&
                        !type.IsAbstract));
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Add logging
            serviceCollection.AddSingleton(new LoggerFactory())
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
            serviceCollection.AddSingleton(Configuration);

            // Add dbContext
            serviceCollection.AddDbContext<MpkContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MpkContext")));

            // Add services
            serviceCollection.AddTransient(typeof(IGenericRepository<>), typeof(BaseRepository<>));

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(serviceCollection);

            var entityBuilders = GetAllTypes(typeof(IEntityBuilder<>));
            foreach (var entityBuilder in entityBuilders)
            {
                var interfaceType = entityBuilder.GetInterfaces().FirstOrDefault();
                containerBuilder.RegisterType(entityBuilder).As(interfaceType);
            }

            containerBuilder.RegisterGeneric(typeof(ImporterService<>)).As(typeof(IImporterService<>));
            containerBuilder.RegisterType(typeof(ShapeImporterService)).As(typeof(IImporterService<Shape>));
            containerBuilder.RegisterType(typeof(ShapeCollectionHelper)).As(typeof(IShapeCollectionHelper));

            Container = containerBuilder.Build();
        }

        private static IEnumerable<Type> GetAllTypesOf<T>()
        {
            var platform = Environment.OSVersion.Platform.ToString();
            var runtimeAssemblyNames = DependencyContext.Default.GetRuntimeAssemblyNames(platform);

            return runtimeAssemblyNames
                .Select(Assembly.Load)
                .SelectMany(a => a.ExportedTypes)
                .Where(t => typeof(T).IsAssignableFrom(t));
        }

        private static void Main(string[] args)
        {
            // Create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Process data
            var fileNames = Configuration.GetSection("GTFS").GetChildren().ToDictionary(kv => kv.Key, kv => kv.Value);
            var entityModelTypes = GetAllTypesOf<IdentifiableEntity<string>>();

            using (var scope = Container.BeginLifetimeScope())
            {
                foreach (var entityType in entityModelTypes)
                {
                    if (!fileNames.ContainsKey(entityType.Name))
                    {
                        continue;
                    }
                    var fileName = fileNames[entityType.Name];
                    if (!File.Exists(fileName))
                    {
                        continue;
                    }

                    var interfaceType = typeof(IImporterService<>).MakeGenericType(entityType);
                    var entityImporter = scope.Resolve(interfaceType) as IEntityImporter;

                    entityImporter?.ImportEntitiesFromFile(fileName);
                    System.Console.WriteLine($"Processing of {entityType.Name} finished. Press ENTER to continue");
                    System.Console.ReadKey();
                }
            }
            System.Console.ReadLine();
        }
    }
}