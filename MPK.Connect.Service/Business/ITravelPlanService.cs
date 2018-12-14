using MPK.Connect.Model.Business.TravelPlan;

namespace MPK.Connect.Service.Business
{
    public interface ITravelPlanService
    {
        TravelPlan GetTravelPlan(Location sourceLocation, Location destinationLocation);
    }
}