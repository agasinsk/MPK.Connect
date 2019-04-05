using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Service.Business.HarmonySearch.Functions;

namespace MPK.Connect.Console
{
    public class ObjectiveFunctionFactory
    {
        public static IObjectiveFunction<StopTimeInfo> GetInstance(ObjectiveFunctionTypes type, Location destination)
        {
            switch (type)
            {
                case ObjectiveFunctionTypes.TravelTime:
                    return new TravelTimeObjectiveFunction(destination);

                case ObjectiveFunctionTypes.Transfers:
                    return new TransfersObjectiveFunction(destination);

                case ObjectiveFunctionTypes.Comprehensive:
                    return new ComprehensiveObjectiveFunction(destination);

                default:
                    return new ComprehensiveObjectiveFunction(destination);
            }
        }
    }
}