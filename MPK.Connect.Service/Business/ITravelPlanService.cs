using System.Collections.Generic;
using MPK.Connect.Model.Business.TravelPlan;

namespace MPK.Connect.Service.Business
{
    public interface ITravelPlanService
    {
        IEnumerable<TravelPlan> GetTravelPlans(Location source, Location destination);
    }
}