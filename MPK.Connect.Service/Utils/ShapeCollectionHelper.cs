using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Model;
using MPK.Connect.Service.Helpers;

namespace MPK.Connect.Service.Utils
{
    public class ShapeCollectionHelper : IShapeCollectionHelper
    {
        public List<ShapeBase> GetShapeBases(List<Shape> shapes)
        {
            var shapeBases = new List<ShapeBase>();

            foreach (var shape in shapes)
            {
                if (shapeBases.All(sb => sb.Id != shape.ShapeId))
                {
                    shapeBases.Add(new ShapeBase { Id = shape.ShapeId });
                }
            }

            return shapeBases;
        }

        public List<ShapeBase> GetShapeBasesByGrouping(List<Shape> shapes)
        {
            var shapeBases = shapes.GroupBy(s => s.ShapeId).Select(g => new ShapeBase { Id = g.First().ShapeId }).ToList();

            return shapeBases;
        }

        public Dictionary<ShapeBase, List<Shape>> GroupByShapeId(List<Shape> shapes)
        {
            var shapeBases = shapes
                .GroupBy(s => s.ShapeId)
                .Select(g => g.OrderBy(point => point.PointSequence))
                .ToDictionary(sps => new ShapeBase { Id = sps.First().ShapeId }, value => value.ToList());

            return shapeBases;
        }
    }
}