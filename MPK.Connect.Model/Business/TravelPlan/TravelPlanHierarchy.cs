using System.Collections.Generic;

namespace MPK.Connect.Model.Business.TravelPlan
{
    public class TravelPlanHierarchy
    {
        public Dictionary<TravelPlanOptimalities, IEnumerable<TravelPlan>> TravelPlans;
    }
}