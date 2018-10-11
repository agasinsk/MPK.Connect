using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;

namespace MPK.Connect.Service
{
    public abstract class ImporterService<T> : IImporterService where T : class
    {
        private readonly ILogger<ImporterService<T>> _logger;
        private readonly IGenericRepository<T> _repository;

        protected ImporterService(IGenericRepository<T> repository, ILogger<ImporterService<T>> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
        }

        public int ImportEntitiesFromFile(string filePath)
        {
            var entities = new List<T>();
            int entitiesCount;

            using (var streamReader = new StreamReader(filePath))
            {
                var entityLine = streamReader.ReadLine();

                while ((entityLine = streamReader.ReadLine()) != null)
                {
                    var mappedEntity = Map(entityLine);
                    entities.Add(mappedEntity);
                }
                _logger.LogInformation($"Read {entities.Count} lines of {nameof(T)}.");
                SortEntities(entities);

                _repository.AddRange(entities);
                _repository.Save();
                entitiesCount = entities.Count;
                entities.Clear();
                _logger.LogInformation($"{nameof(T)} have been successfully saved!");
            }

            return entitiesCount;
        }

        protected abstract T Map(string entityString);

        protected virtual void SortEntities(List<T> entities)
        {
        }
    }
}