namespace MPK.Connect.Service.Business.HarmonySearch
{
    public interface IGeneralObjectiveFunction<T> : IObjectiveFunction<T>
    {
        /// <summary>
        /// Gets a collection of random arguments
        /// </summary>
        /// <remarks>Note. This method does not rely on constant argument count (or their indexes)</remarks>
        /// <returns>Random arguments</returns>
        T[] GetRandomArguments();
    }
}