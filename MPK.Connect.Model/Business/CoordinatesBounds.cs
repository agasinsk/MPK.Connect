namespace MPK.Connect.Model.Business
{
    public class CoordinatesBounds
    {
        public double MaxLatitude { get; set; }

        public double MaxLongitude { get; set; }
        public double MinLatitude { get; set; }
        public double MinLongitude { get; set; }

        public CoordinatesBounds()
        {
        }

        public CoordinatesBounds(double maxLatitude, double maxLongitude, double minLatitude, double minLongitude)
        {
            MaxLatitude = maxLatitude;
            MaxLongitude = maxLongitude;
            MinLatitude = minLatitude;
            MinLongitude = minLongitude;
        }
    }
}