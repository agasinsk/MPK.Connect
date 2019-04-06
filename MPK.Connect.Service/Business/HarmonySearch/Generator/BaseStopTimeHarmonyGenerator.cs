using System;
using System.Collections.Generic;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Business.Graph;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;

namespace MPK.Connect.Service.Business.HarmonySearch.Generator
{
    /// <summary>
    /// Base stop time harmony generator
    /// </summary>
    public abstract class BaseStopTimeHarmonyGenerator : BaseHarmonyGenerator<StopTimeInfo>
    {
        protected readonly Graph<int, StopTimeInfo> Graph;
        protected readonly StopDto ReferentialDestinationStop;
        protected readonly List<GraphNode<int, StopTimeInfo>> SourceNodes;

        public Location Destination { get; }

        public Location Source { get; }

        protected BaseStopTimeHarmonyGenerator(IObjectiveFunction<StopTimeInfo> function, Graph<int, StopTimeInfo> graph, Location source, Location destination) : base(function)
        {
            Graph = graph ?? throw new ArgumentNullException(nameof(graph));
            Destination = destination ?? throw new ArgumentNullException(nameof(destination));
            Source = source ?? throw new ArgumentNullException(nameof(source));

            // Set up source and destination nodes
            ReferentialDestinationStop = graph.GetReferenceDestinationStop(Destination.Name);
            SourceNodes = graph.GetSourceNodes(Source.Name, ReferentialDestinationStop);
        }

        public override Harmony<StopTimeInfo> GenerateRandomHarmony()
        {
            var randomArguments = GetRandomArguments();

            var objectiveValue = ObjectiveFunction.GetObjectiveValue(randomArguments);

            return new Harmony<StopTimeInfo>(objectiveValue, randomArguments);
        }

        protected abstract StopTimeInfo[] GetRandomArguments();
    }
}