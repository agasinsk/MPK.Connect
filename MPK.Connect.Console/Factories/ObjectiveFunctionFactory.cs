using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Service.Business.HarmonySearch.Functions;

namespace MPK.Connect.TestEnvironment.Factories
{
    public class ObjectiveFunctionFactory
    {
        public static IObjectiveFunction<StopTimeInfo> GetInstance(ObjectiveFunctionType type, Location destination)
        {
            switch (type)
            {
                case ObjectiveFunctionType.TravelTime:
                    return new TravelTimeObjectiveFunction(destination);

                case ObjectiveFunctionType.Transfers:
                    return new TransfersObjectiveFunction(destination);

                case ObjectiveFunctionType.Comprehensive:
                    return new ComprehensiveObjectiveFunction(destination);

                default:
                    return new ComprehensiveObjectiveFunction(destination);
            }
        }
    }
}