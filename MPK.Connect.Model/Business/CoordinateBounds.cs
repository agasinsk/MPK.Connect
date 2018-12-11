namespace MPK.Connect.Model.Business
{
    public class CoordinateBounds
    {
        public double MaxLatitude { get; set; }

        public double MaxLongitude { get; set; }
        public double MinLatitude { get; set; }
        public double MinLongitude { get; set; }

        public CoordinateBounds()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="maxLatitude"></param>
        /// <param name="maxLongitude"></param>
        /// <param name="minLatitude"></param>
        /// <param name="minLongitude"></param>
        public CoordinateBounds(double maxLatitude, double maxLongitude, double minLatitude, double minLongitude)
        {
            MaxLatitude = maxLatitude;
            MaxLongitude = maxLongitude;
            MinLatitude = minLatitude;
            MinLongitude = minLongitude;
        }
    }
}