namespace MPK.Connect.Service.Business.HarmonySearch.Functions
{
    /// <summary>
    /// The interface for objective function
    /// </summary>
    /// <typeparam name="T">Type of arguments</typeparam>
    public interface IObjectiveFunction<in T>
    {
        /// <summary>
        /// Gets the type of objective function
        /// </summary>
        ObjectiveFunctionType Type { get; }

        /// <summary>
        /// Return the objective function value given a solution vector containing each decision
        /// variable. In practice, vector should be a list of parameters.
        /// </summary>
        /// <param name="arguments">Input vector of decision variables</param>
        /// <returns>Objective function value</returns>
        double GetObjectiveValue(params T[] arguments);
    }
}