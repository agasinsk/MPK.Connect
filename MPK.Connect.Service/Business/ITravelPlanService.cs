using MPK.Connect.Model.Business.TravelPlan;

namespace MPK.Connect.Service.Business
{
    public interface ITravelPlanService
    {
        string GetTravelPlan(Location sourceLocation, Location destinationLocation);
    }
}