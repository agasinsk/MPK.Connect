using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
using MPK.Connect.Service.Builders;
using MPK.Connect.Service.Helpers;
using MPK.Connect.Service.Import;
using MPK.Connect.Service.Utils;

namespace MPK.Console.DataImporter
{
    public class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static IContainer Container { get; set; }

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
            serviceCollection.AddDbContext<MpkContext>(options => options.UseSqlServer(Configuration.GetConnectionString(nameof(MpkContext))))
                .AddDbContext<SimpleMpkContext>(options => options.UseSqlServer(Configuration.GetConnectionString(nameof(SimpleMpkContext))));

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
            containerBuilder.RegisterType(typeof(StopTimeImporterService)).As(typeof(IImporterService<StopTime>));
            containerBuilder.RegisterType(typeof(ShapeCollectionHelper)).As(typeof(IShapeCollectionHelper));

            containerBuilder.RegisterType(typeof(SimpleMpkContext)).As<IMpkContext>();

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

        private static Dictionary<Type, string> GetDataSources()
        {
            var filePaths = GetDataSourcesFromConfiguration();
            var dbContextEntityTypes = GetDbContextEntityTypes();

            var entityModelTypes = GetAllTypesOf<IIdentifiable>()
                    .Where(t => dbContextEntityTypes.Contains(t))
                    .ToDictionary(kv => kv.Name, kv => kv);

            return filePaths.Where(f => entityModelTypes.ContainsKey(f.Key) && File.Exists(f.Value))
                .ToDictionary(k => entityModelTypes[k.Key], v => v.Value);
        }

        private static Dictionary<string, string> GetDataSourcesFromConfiguration()
        {
            return Configuration.GetSection("GTFS").GetChildren().Select(c =>
            {
                var gtfsEntity = new GtfsEntity();
                c.Bind(gtfsEntity);
                return gtfsEntity;
            }).OrderBy(g => g.Order).ToDictionary(kv => kv.Name, kv => kv.FilePath);
        }

        private static List<Type> GetDbContextEntityTypes()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var dbContextType = scope.Resolve<IMpkContext>().GetType();
                var dbContextEntityTypes = dbContextType.GetProperties()
                    .Where(p => p.PropertyType.GenericTypeArguments.Length > 0)
                    .Select(p => p.PropertyType.GenericTypeArguments.First()).ToList();
                return dbContextEntityTypes;
            }
        }

        private static void Main(string[] args)
        {
            // Create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Process data
            var dataSources = GetDataSources();

            using (var scope = Container.BeginLifetimeScope())
            {
                foreach (var data in dataSources)
                {
                    var entityType = data.Key;
                    var filePath = data.Value;

                    var interfaceType = typeof(IImporterService<>).MakeGenericType(entityType);
                    var entityImporter = scope.Resolve(interfaceType) as IEntityImporter;
                    var entitiesSaved = entityImporter?.ImportEntitiesFromFile(filePath);

                    System.Console.WriteLine($"Processing of {entitiesSaved} {entityType.Name} finished.");
                }
            }
            System.Console.WriteLine("Processing finished. Press ENTER to continue");
            System.Console.ReadLine();
        }
    }
}