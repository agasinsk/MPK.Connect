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
            var shapePtLat = GetDouble(data[_entityMappings["shape_pt_lat"]]).GetValueOrDefault();
            var shapePtLon = GetDouble(data[_entityMappings["shape_pt_lon"]]).GetValueOrDefault();
            var shapePtSequence = GetInt(data[_entityMappings["shape_pt_sequence"]]).GetValueOrDefault();
            var shapeDistTraveled = GetDouble(data[_entityMappings["shape_dist_traveled"]]);

            var shapePoint = new Shape
            {
                ShapeId = shapeId,
                PointLatitude = shapePtLat,
                PointLongitude = shapePtLon,
                PointSequence = shapePtSequence,
                DistTraveled = shapeDistTraveled
            };
            return shapePoint;
        }
    }
}