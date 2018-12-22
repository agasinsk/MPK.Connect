using System.Collections.Generic;
using MPK.Connect.Model.Business.TravelPlan;

namespace MPK.Connect.Service.Business
{
    public interface ITravelPlanService
    {
        Dictionary<TravelPlanOptimalities, IEnumerable<TravelPlan>> GetTravelPlans(TravelOptions travelOptions);
    }
}