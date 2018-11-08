using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess;
using MPK.Connect.Model.Helpers;
using MPK.Connect.Service.Builders;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MPK.Connect.Service.Import
{
    public class ImporterService<T> : IImporterService<T> where T : IdentifiableEntity<string>
    {
        protected readonly IGenericRepository<T> _repository;
        protected readonly IEntityBuilder<T> _entityBuilder;
        private readonly ILogger<ImporterService<T>> _logger;

        public ImporterService(IGenericRepository<T> repository, IEntityBuilder<T> entityBuilder, ILogger<ImporterService<T>> logger)
        {
            _repository = repository;
            _entityBuilder = entityBuilder;
            _logger = logger;
        }

        public int ImportEntitiesFromFile(string filePath)
        {
            var existingEntitiesCount = _repository.GetAll().Count();
            if (existingEntitiesCount > 0)
            {
                _logger.LogInformation($"Entities of type '{typeof(T).Name}' have been already imported.");
                return 0;
            }

            _logger.LogInformation($"Reading file with {typeof(T).Name}s . . .");

            var entityLines = File.ReadLines(filePath).ToList();
            _entityBuilder.ReadEntityMappings(entityLines.First());
            entityLines.RemoveAt(0);

            _logger.LogInformation($"Mapping to {typeof(T).Name} objects . . .");
            var mappedEntities = entityLines.Select(line => _entityBuilder.Build(line)).ToList();

            _logger.LogInformation($"Read {mappedEntities.Count} lines of {typeof(T).Name}.");
            SortEntities(mappedEntities);
            var entitiesCount = SaveEntities(mappedEntities);

            _logger.LogInformation($"{entitiesCount} entities of type '{typeof(T).Name}' have been successfully saved.");

            return entitiesCount;
        }

        protected virtual void SortEntities(List<T> entities)
        {
        }

        protected virtual int SaveEntities(List<T> entities)
        {
            _logger.LogInformation($"Saving entities of type {typeof(T).Name} in the database . . .");
            return _repository.BulkInsert(entities);
        }
    }
}