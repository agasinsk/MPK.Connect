using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess;
using MPK.Connect.Model;
using MPK.Connect.Service.Builders;
using MPK.Connect.Service.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace MPK.Connect.Service
{
    public class ShapePointImporterService : ImporterService<ShapePoint>
    {
        private readonly IGenericRepository<Shape> _shapeRepository;

        public ShapePointImporterService(IGenericRepository<ShapePoint> repository, IGenericRepository<Shape> shapeRepository, IEntityBuilder<ShapePoint> entityBuilder, ILogger<ImporterService<ShapePoint>> logger) : base(repository, entityBuilder, logger)
        {
            _shapeRepository = shapeRepository;
        }

        protected override int SaveEntities(List<ShapePoint> shapePoints)
        {
            var distinctShapes = shapePoints.Select(sp => new Shape { Id = sp.ShapeId }).Distinct(new ShapeComparer()).ToList();

            _shapeRepository.BulkMerge(distinctShapes);
            return _repository.BulkMerge(shapePoints);
        }
    }
}