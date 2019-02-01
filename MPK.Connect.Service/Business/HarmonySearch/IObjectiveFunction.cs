namespace MPK.Connect.Service.Business.HarmonySearch
{
    public interface IObjectiveFunction<T> : ICalculator<T>
    {
        /// <summary>
        /// Return the objective function value given a solution vector containing each decision
        /// variable. In practice, vector should be a list of parameters.
        /// </summary>
        /// <param name="arguments">Input vector of decision variables</param>
        /// <returns>Objective function value</returns>
        double CalculateObjectiveValue(params T[] arguments);

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
        /// <param name="discreteValueIndex">Index of discrete value</param>
        /// <returns>Valid value of parameter</returns>
        T GetArgumentValue(int argumentIndex, int? discreteValueIndex = null);

        /// <summary>
        /// Gets the index of the value argumentValue of the specified parameter.
        ///
        /// This will only be called for discrete variables in the pitch adjustment step. The
        /// behavior here isn't well-defined in the case where the possible values for a variable
        /// contain non-unique elements. For best performance, store discrete values in a sorted list
        /// that can be binary searched. Additionally, this list should not contain any duplicate values.
        /// </summary>
        /// <param name="argumentIndex">Index of an argument</param>
        /// <param name="argumentValue">Value of the discrete argument</param>
        /// <returns></returns>
        int GetIndexOfDiscreteValue(int argumentIndex, T argumentValue);

        /// <summary>
        /// Return the lower bound of parameter i. This will only be called for continuous variables
        /// in the pitch adjustment step.
        /// </summary>
        /// <param name="argumentIndex">Index of an argument</param>
        /// <returns>Lower bound</returns>
        T GetLowerBound(int argumentIndex);

        /// <summary>
        /// Return the maximum pitch adjustment proportion. This determines the range from which
        /// pitch adjustment may occur for continuous variables. Also known as continuous bandwidth.
        /// </summary>
        /// <returns>Maximum pitch adjustment proportion.</returns>
        T GetMaximumContinuousPitchAdjustmentProportion();

        /// <summary>
        /// Return the maximum pitch adjustment index. This determines the range from which pitch
        /// adjustment may occur for discrete variables. Also known as discrete bandwidth.
        /// </summary>
        int GetMaximumDiscretePitchAdjustmentIndex();

        /// <summary>
        /// Get the number of values possible for the discrete parameter argumentIndex.
        ///
        /// This will only be called for discrete variables in the pitch adjustment step. If i is a
        /// continuous variable, +inf can be returned, but this function might not be implemented for
        /// continuous variables, so this shouldn't be counted on.
        /// </summary>
        /// <param name="argumentIndex">Index of an argument</param>
        /// <returns></returns>
        int GetPossibleDiscreteValuesCount(int argumentIndex);

        /// <summary>
        /// Return the upper bound of parameter i. This will only be called for continuous variables
        /// in the pitch adjustment step.
        /// </summary>
        /// <param name="argumentIndex">Index of an argument</param>
        /// <returns></returns>
        T GetUpperBound(int argumentIndex);

        /// <summary>
        /// Return whether or not the parameter at the specified index is a discrete parameter. Not
        /// all parameters may be continuous.This only really matters in the pitch adjustment step of HS.
        /// </summary>
        /// <param name="argumentIndex">Index of an argument</param>
        /// <returns></returns>
        bool IsArgumentDiscrete(int argumentIndex);

        /// <summary>
        /// Return whether or not the parameter at the specified index should be varied by HS. It may
        /// be the case that HS should only vary certain parameters while others should remain fixed.
        /// </summary>
        /// <param name="argumentIndex">Index of an argument</param>
        /// <returns></returns>
        bool IsArgumentVariable(int argumentIndex);
    }
}