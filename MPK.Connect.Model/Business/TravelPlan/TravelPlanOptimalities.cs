using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MPK.Connect.Model.Business.TravelPlan
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TravelPlanOptimalities
    {
        Fast = 1,
        Comfortable = 2
    }
}