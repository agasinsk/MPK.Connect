namespace MPK.Connect.Model.Business
{
    public class StopTimeDto : StopTimeCore
    {
        public string StopId { get; set; }

        public override string ToString()
        {
            return $"{nameof(TripId)}: {TripId}, {nameof(StopId)}: {StopId}, {nameof(DepartureTime)}: {DepartureTime}";
        }
    }
}