using MPK.Connect.Model;

namespace MPK.Connect.Service.Builders
{
    public class ShapeBuilder : BaseEntityBuilder<Shape>
    {
        public override Shape Build(string dataString)
        {
            var data = dataString.Replace("\"", "").Split(',');

            var shapeId = data[_entityMappings["shape_id"]];
            var shapePtLat = double.Parse(data[_entityMappings["shape_pt_lat"]]);
            var shapePtLon = double.Parse(data[_entityMappings["shape_pt_lon"]]);
            var shapePtSequence = int.Parse(data[_entityMappings["shape_pt_sequence"]]);
            var shapeDistTraveled = _entityMappings.ContainsKey("shape_dist_traveled") ? double.Parse(data[_entityMappings["shape_dist_traveled"]]) : (double?)null;

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