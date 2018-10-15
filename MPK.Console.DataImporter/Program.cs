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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MPK.Console.DataImporter
{
    public class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        private static IContainer Container { get; set; }

        public static IEnumerable<Type> GetAllTypes(Type genericType)
        {
            var platform = Environment.OSVersion.Platform.ToString();
            var runtimeAssemblyNames = DependencyContext.Default.GetRuntimeAssemblyNames(platform);

            return runtimeAssemblyNames
                .Select(Assembly.Load)
                .SelectMany(assembly => assembly.GetTypes()
                    .Where(type => type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericType) && !type.IsAbstract));
        }

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
            var entityBuilders = GetAllTypes(typeof(IEntityBuilder<>));
            foreach (var entityBuilder in entityBuilders)
            {
                serviceCollection.AddTransient(typeof(IEntityBuilder<IdentifiableEntity<string>>), entityBuilder);
            }

            serviceCollection.AddTransient(typeof(IGenericRepository<>), typeof(BaseRepository<>));
            serviceCollection.AddTransient(typeof(IImporterService<>), typeof(ImporterService<>));
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

            // Create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var fileNames = Configuration.GetSection("GTFS").GetChildren().ToDictionary(kv => kv.Key, kv => kv.Value);
            var entityModelTypes = GetAllTypesOf<IdentifiableEntity<string>>();

            foreach (var entityType in entityModelTypes)
            {
                var fileName = fileNames[entityType.Name];

                var entityImporter = serviceProvider.GetService(typeof(IImporterService<Stop>)) as IImporterService<Stop>;
                //entityImporter?.ImportEntitiesFromFile(fileName);
            }

            // Get backup sources for client
            System.Console.ReadLine();
        }
    }
}