namespace MPK.Connect.Service.Business.HarmonySearch.Functions
{
    public interface IArgumentObjectiveFunction<T> : IObjectiveFunction<T>
    {
        /// <summary>
        /// Returns the number of parameters used by the objective function.
        /// </summary>
        /// <returns>Number of parameters used by the objective function.</returns>
        int GetArgumentsCount();

        /// <summary>
        /// Get a valid value of parameter argumentIndex. You can return values any way you like -
        /// uniformly at random, according to some distribution, etc.
        ///
        /// discreteValueIndex is used only for discrete parameters in the pitch adjustment step.
        /// discreteValueIndex maps to some value the discrete parameter can take on. If parameter
        /// argumentIndex is continuous, discreteValueIndex should be ignored.
        /// </summary>
        /// <param name="argumentIndex">Index of argument</param>
        /// <returns>Valid value of parameter</returns>
        T GetArgumentValue(int argumentIndex);

        /// <summary>
        /// Check if the argument value is possible to be used
        ///
        /// Used in problems that require the solutions to have distinct argument values
        /// </summary>
        /// <param name="argumentValue">Argument value to check</param>
        /// <returns>If the argument value is possible to be used</returns>
        bool IsArgumentValuePossible(T argumentValue);
    }
}