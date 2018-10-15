using MPK.Connect.Model;
using System.Collections.Generic;

namespace MPK.Connect.Service.Builders
{
    public class ShapeBuilder : BaseEntityBuilder<Shape>
    {
        public override Shape Build(string dataString, IDictionary<string, int> mappings)
        {
            var data = dataString.Replace("\"", "").Split(',');

            var shapeId = data[mappings["shape_id"]];
            var shapePtLat = double.Parse(data[mappings["shape_pt_lat"]]);
            var shapePtLon = double.Parse(data[mappings["shape_pt_lon"]]);
            var shapePtSequence = int.Parse(data[mappings["shape_pt_sequence"]]);
            var shapeDistTraveled = mappings.ContainsKey("shape_dist_traveled") ? double.Parse(data[mappings["shape_dist_traveled"]]) : (double?)null;

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