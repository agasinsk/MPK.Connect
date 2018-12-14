using System;

namespace MPK.Connect.Model.Business.TravelPlan
{
    public class TravelPlanjakDojade
    {
        public string routeDescription { get; set; }
        public string routeId { get; set; }
        public Routepart[] routeParts { get; set; }
        public TravelTypes Type { get; set; }

        public class Alternativeline
        {
            public object[] alternativeLines { get; set; }
            public bool courseLine { get; set; }
            public Line line { get; set; }
            public string lineHeadingText { get; set; }
            public bool notMainVariant { get; set; }
            public string realtimeStatus { get; set; }
            public Transportoperator transportOperator { get; set; }
        }

        public class Coordinate
        {
            public float x_lon { get; set; }
            public float y_lat { get; set; }
        }

        public class Line
        {
            public string lineId { get; set; }
            public object[] lineTypes { get; set; }
            public string name { get; set; }
            public string vehicleType { get; set; }
        }

        public class Routeline
        {
            public Alternativeline[] alternativeLines { get; set; }
            public string courseId { get; set; }
            public bool courseLine { get; set; }
            public int departuresPeriodMinutes { get; set; }
            public Line line { get; set; }
            public string lineHeadingText { get; set; }
            public bool notMainVariant { get; set; }
            public string realtimeStatus { get; set; }
            public Transportoperator transportOperator { get; set; }
        }

        public class Routepart
        {
            public int routePartDistanceMeters { get; set; }
            public string routePartType { get; set; }
            public Routevehicle routeVehicle { get; set; }
            public DateTime startDeparture { get; set; }
            public DateTime targetArrival { get; set; }
        }

        public class Routestop
        {
            public DateTime arrivalTime { get; set; }
            public DateTime departureTime { get; set; }
            public Location location { get; set; }
            public Shape[] shape { get; set; }
        }

        public class Routevehicle
        {
            public Routeline routeLine { get; set; }
            public Routestop[] routeStops { get; set; }
            public string routeVehicleType { get; set; }
            public int stopsEndIndex { get; set; }
            public int stopsStartIndex { get; set; }
        }

        public class Transportoperator
        {
            public string nameShortcut { get; set; }
            public int transportOperatorId { get; set; }
            public string transportOperatorName { get; set; }
            public string transportOperatorSymbol { get; set; }
        }
    }
}