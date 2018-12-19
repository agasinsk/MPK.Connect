namespace MPK.Connect.Model.Business
{
    public class CoordinateLimits
    {
        public double UpperLeftLatitude { get; set; }

        public double UpperLeftLongitude { get; set; }
        public double LowerRightLatitude { get; set; }
        public double LowerRightLongitude { get; set; }

        public CoordinateLimits()
        {
        }

        public CoordinateLimits(double upperLeftLatitude, double upperLeftLongitude, double lowerRightLatitude, double lowerRightLongitude)
        {
            UpperLeftLatitude = upperLeftLatitude;
            UpperLeftLongitude = upperLeftLongitude;
            LowerRightLatitude = lowerRightLatitude;
            LowerRightLongitude = lowerRightLongitude;
        }

        public void Add(double number)
        {
            UpperLeftLatitude += number;
            UpperLeftLongitude -= number;
            LowerRightLatitude -= number;
            LowerRightLongitude += number;
        }
    }
}