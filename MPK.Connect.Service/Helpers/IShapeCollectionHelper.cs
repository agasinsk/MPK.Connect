using MPK.Connect.Model;
using System.Collections.Generic;

namespace MPK.Connect.Service.Helpers
{
    public interface IShapeCollectionHelper
    {
        List<ShapeBase> GetShapeBases(List<Shape> shapes);

        List<ShapeBase> GetShapeBasesByGrouping(List<Shape> shapes);

        Dictionary<ShapeBase, List<Shape>> GroupByShapeId(List<Shape> shapes);
    }
}