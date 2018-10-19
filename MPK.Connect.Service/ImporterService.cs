using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess;
using MPK.Connect.Model.Helpers;
using MPK.Connect.Service.Builders;
using System.Collections.Generic;
using System.IO;

namespace MPK.Connect.Service
{
    public class ImporterService<T> : IImporterService<T> where T : IdentifiableEntity<string>
    {
        private readonly ILogger<ImporterService<T>> _logger;
        private readonly IGenericRepository<T> _repository;
        private readonly IEntityBuilder<T> _entityBuilder;

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
                var entityMappings = _entityBuilder.GetEntityMappings(entityLine);

                while ((entityLine = streamReader.ReadLine()) != null)
                {
                    var mappedEntity = _entityBuilder.Build(entityLine, entityMappings);
                    entities.Add(mappedEntity);
                    _logger.LogInformation($"Read {typeof(T).Name} with id: \"{mappedEntity.Id}\"");
                }
                _logger.LogInformation($"Read {entities.Count} lines of {typeof(T).Name}.");
                SortEntities(entities);

                entitiesCount = _repository.AddRange(entities);
                _repository.Save();
            }

            _logger.LogInformation($"{entitiesCount} entities of type '{typeof(T).Name}' have been successfully saved.");

            return entitiesCount;
        }

        protected virtual void SortEntities(List<T> entities)
        {
        }
    }
}