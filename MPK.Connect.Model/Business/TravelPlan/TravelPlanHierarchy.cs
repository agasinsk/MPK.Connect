using System.Collections.Generic;

namespace MPK.Connect.Model.Business.TravelPlan
{
    public class TravelPlanHierarchy
    {
        public Dictionary<TravelPlanCategories, IEnumerable<TravelPlan>> TravelPlans;
    }
}