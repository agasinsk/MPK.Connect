using System.Collections.Generic;
using MPK.Connect.Model.Business.TravelPlan;

namespace MPK.Connect.Service.Business
{
    public interface ITravelPlanService
    {
        Dictionary<TravelPlanCategories, IEnumerable<TravelPlan>> GetTravelPlans(TravelOptions travelOptions);
    }
}