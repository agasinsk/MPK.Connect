using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess;
using MPK.Connect.Model;
using MPK.Connect.Service.Builders;
using MPK.Connect.Service.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace MPK.Connect.Service.Import
{
    public class ShapeImporterService : ImporterService<Shape>
    {
        private readonly IShapeCollectionHelper _shapeCollectionHelper;
        private readonly IGenericRepository<ShapeBase> _shapeBaseRepository;

        public ShapeImporterService(IGenericRepository<Shape> repository, IGenericRepository<ShapeBase> shapeBaseRepository, IEntityBuilder<Shape> entityBuilder, ILogger<ImporterService<Shape>> logger, IShapeCollectionHelper shapeCollectionHelper) : base(repository, entityBuilder, logger)
        {
            _shapeBaseRepository = shapeBaseRepository;
            _shapeCollectionHelper = shapeCollectionHelper;
        }

        protected override int SaveEntities(List<Shape> shapes)
        {
            var groupedShapes = _shapeCollectionHelper.GroupByShapeId(shapes);

            var shapeBases = groupedShapes.Keys.ToList();
            _shapeBaseRepository.BulkInsert(shapeBases);

            var shapePoints = groupedShapes.SelectMany(s => s.Value).ToList();
            return _repository.BulkInsert(shapePoints);
        }
    }
}