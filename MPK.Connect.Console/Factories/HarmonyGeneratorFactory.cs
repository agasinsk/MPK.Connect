using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Business.HarmonySearch.Generator;

namespace MPK.Connect.TestEnvironment.Factories
{
    public class HarmonyGeneratorFactory
    {
        public static IHarmonyGenerator<StopTimeInfo> GetInstance(HarmonyGeneratorType type, IObjectiveFunction<StopTimeInfo> function, Graph<int, StopTimeInfo> graph, Location source, Location destination)
        {
            switch (type)
            {
                case HarmonyGeneratorType.RandomStopTime:
                    return new RandomStopTimeHarmonyGenerator(function, graph, source, destination);

                case HarmonyGeneratorType.RandomStop:
                    return new RandomStopHarmonyGenerator(function, graph, source, destination);

                case HarmonyGeneratorType.RandomDirectedStop:
                    return new DirectedStopTimeHarmonyGenerator(function, graph, source, destination);

                default:
                    return new DirectedStopTimeHarmonyGenerator(function, graph, source, destination);
            }
        }
    }
}