using System.Collections.Generic;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;

namespace MPK.Connect.Service.Business.HarmonySearch.Generator
{
    public class AntColonyHarmonyGenerator<T> : GeneralHarmonyGenerator<T>
    {
        protected new IAntColonyObjectiveFunction<T> ObjectiveFunction;
        private readonly Dictionary<int, Dictionary<int, double>> _pheromoneAmounts;

        public AntColonyHarmonyGenerator(IObjectiveFunction<T> function, HarmonyMemory<T> harmonyMemory) : base(function, harmonyMemory)
        {
        }

        public AntColonyHarmonyGenerator(IAntColonyObjectiveFunction<T> function, HarmonyMemory<T> harmonyMemory, double harmonyMemoryConsiderationRatio, double pitchAdjustmentRatio) : base(function, harmonyMemory, harmonyMemoryConsiderationRatio, pitchAdjustmentRatio)
        {
        }

        public override IEnumerable<Harmony<T>> GetAntSolutions()
        {
            return ObjectiveFunction.GetAntSolutions();
        }
    }
}