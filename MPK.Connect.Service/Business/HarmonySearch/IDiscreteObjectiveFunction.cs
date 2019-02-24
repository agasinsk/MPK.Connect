namespace MPK.Connect.Service.Business.HarmonySearch
{
    public interface IDiscreteObjectiveFunction<T> : IArgumentObjectiveFunction<T>
    {
        int GetIndexOfDiscreteValue(int argumentIndex, T argumentValue);

        T GetNeighborValue(int argumentIndex, T valueFromHarmonyMemory);

        /// <summary>
        /// Saves the argument value in function if there's a need to provide distinct values of
        /// arguments to the solution
        ///
        /// Used when Harmony Memory consideration scenario is specified.
        /// </summary>
        /// <param name="argumentIndex">Argument index</param>
        /// <param name="argumentValue">Argument value</param>
        void SaveArgumentValue(int argumentIndex, T argumentValue);
    }
}