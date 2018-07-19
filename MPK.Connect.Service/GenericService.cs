using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess;

namespace MPK.Connect.Service
{
    public abstract class GenericService<T> : IGenericService<T> where T : class
    {
        private readonly ILogger<GenericService<T>> _logger;
        private readonly IGenericRepository<T> _repository;

        protected GenericService(IGenericRepository<T> repository, ILogger<GenericService<T>> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
        }

        public int ReadFromFile(string filePath)
        {
            var entities = new List<T>();
            int entitiesCount;

            using (var streamReader = new StreamReader(filePath))
            {
                var entityLine = streamReader.ReadLine();

                while ((entityLine = streamReader.ReadLine()) != null)
                {
                    var createdStop = Map(entityLine);
                    entities.Add(createdStop);
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

        protected abstract void SortEntities(List<T> entities);
    }
}