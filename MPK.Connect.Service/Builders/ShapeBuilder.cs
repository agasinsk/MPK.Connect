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
            var shapePtLat = double.Parse(data[_entityMappings["shape_pt_lat"]]);
            var shapePtLon = double.Parse(data[_entityMappings["shape_pt_lon"]]);
            var shapePtSequence = int.Parse(data[_entityMappings["shape_pt_sequence"]]);
            var shapeDistTraveled = GetDouble(data[_entityMappings["shape_dist_traveled"]]);

            var shape = new Shape
            {
                Id = shapeId,
                PointLatitude = shapePtLat,
                PointLongitude = shapePtLon,
                PointSequence = shapePtSequence,
                DistTraveled = shapeDistTraveled
            };
            return shape;
        }
    }
}