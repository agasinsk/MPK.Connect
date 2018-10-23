using MPK.Connect.Model;
using MPK.Connect.Service.Helpers;

namespace MPK.Connect.Service.Builders
{
    public class ShapeBuilder : BaseEntityBuilder<Shape>
    {
        public override Shape Build(string dataString)
        {
            var data = dataString.Replace("\"", "").ToEntityData();

            var shapeId = data[_entityMappings["shape_id"]];

            var shape = new Shape
            {
                Id = shapeId
            };
            return shape;
        }
    }
}