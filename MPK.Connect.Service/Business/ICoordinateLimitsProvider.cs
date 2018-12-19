using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;

namespace MPK.Connect.Service.Business
{
    public interface ICoordinateLimitsProvider
    {
        CoordinateLimits GetCoordinateLimits(Location sourceLocation, Location destinationLocation);
    }
}