using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess;
using MPK.Connect.Model;
using MPK.Connect.Service.Builders;
using System.Collections.Generic;
using System.Linq;

namespace MPK.Connect.Service.Import
{
    public class StopTimeImporterService : ImporterService<StopTime>
    {
        public StopTimeImporterService(IGenericRepository<StopTime> repository, IEntityBuilder<StopTime> entityBuilder, ILogger<ImporterService<StopTime>> logger) : base(repository, entityBuilder, logger)
        {
        }

        protected override void SortEntities(List<StopTime> entities)
        {
            var ids = Enumerable.Range(1, entities.Count).ToList();
            for (var i = 0; i < entities.Count; i++)
            {
                entities[i].Id = ids[i];
            }
        }
    }
}