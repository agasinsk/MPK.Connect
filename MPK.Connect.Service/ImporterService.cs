using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess;
using MPK.Connect.Model.Helpers;
using MPK.Connect.Service.Builders;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MPK.Connect.Service
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
            var entities = new List<T>();
            int entitiesCount;

            using (var streamReader = new StreamReader(filePath))
            {
                var entityLine = streamReader.ReadLine();
                _entityBuilder.ReadEntityMappings(entityLine);

                while ((entityLine = streamReader.ReadLine()) != null)
                {
                    var mappedEntity = _entityBuilder.Build(entityLine);
                    entities.Add(mappedEntity);
                    _logger.LogInformation($"Read {typeof(T).Name} with id: \"{mappedEntity.Id}\"");
                }
                _logger.LogInformation($"Read {entities.Count} lines of {typeof(T).Name}.");
                SortEntities(entities);
                entitiesCount = SaveEntities(entities);
            }

            _logger.LogInformation($"{entitiesCount} entities of type '{typeof(T).Name}' have been successfully saved.");

            return entitiesCount;
        }

        protected virtual void SortEntities(List<T> entities)
        {
            var hasDistinctId = entities.FirstOrDefault()?.HasDistinctId();
            if (hasDistinctId.GetValueOrDefault())
            {
                entities = entities
                    .GroupBy(t => t.Id)
                    .Select(g => g.First())
                    .ToList();
            }
        }

        protected virtual int SaveEntities(List<T> entities)
        {
            return _repository.BulkMerge(entities);
        }
    }
}