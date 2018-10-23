using MPK.Connect.Model;
using MPK.Connect.Service.Helpers;

namespace MPK.Connect.Service.Builders
{
    public class ShapePointBuilder : BaseEntityBuilder<ShapePoint>
    {
        public override ShapePoint Build(string dataString)
        {
            var data = dataString.Replace("\"", "").ToEntityData();

            var shapeId = data[_entityMappings["shape_id"]];
            var shapePtLat = GetDouble(data[_entityMappings["shape_pt_lat"]]).GetValueOrDefault();
            var shapePtLon = GetDouble(data[_entityMappings["shape_pt_lon"]]).GetValueOrDefault();
            var shapePtSequence = GetInt(data[_entityMappings["shape_pt_sequence"]]).GetValueOrDefault();
            var shapeDistTraveled = GetDouble(data[_entityMappings["shape_dist_traveled"]]);

            var shape = new ShapePoint
            {
                ShapeId = shapeId,
                PointLatitude = shapePtLat,
                PointLongitude = shapePtLon,
                PointSequence = shapePtSequence,
                DistTraveled = shapeDistTraveled
            };
            return shape;
        }
    }
}