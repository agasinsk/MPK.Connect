namespace MPK.Connect.Service.Business.HarmonySearch.Functions
{
    public interface IObjectiveFunction<in T>
    {
        /// <summary>
        /// Gets objective function type
        /// </summary>
        ObjectiveFunctionType Type { get; }

        /// <summary>
        /// Return the objective function value given a solution vector containing each decision
        /// variable. In practice, vector should be a list of parameters.
        /// </summary>
        /// <param name="arguments">Input vector of decision variables</param>
        /// <returns>Objective function value</returns>
        double CalculateObjectiveValue(params T[] arguments);
    }
}