using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Service.Utils;
using System;
using System.Linq;

namespace MPK.Connect.Service.Business.HarmonySearch.Functions
{
    /// <summary>
    /// Objective function that includes travel time only
    /// </summary>
    public class TravelTimeObjectiveFunction : IObjectiveFunction<StopTimeInfo>
    {
        private readonly string _destinationName;

        public ObjectiveFunctionType Type => ObjectiveFunctionType.TravelTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="TravelTimeObjectiveFunction"/> class.
        /// </summary>
        /// <param name="destinationName">Name of the destination.</param>
        /// <exception cref="ArgumentNullException">destinationName</exception>
        public TravelTimeObjectiveFunction(string destinationName) => _destinationName = destinationName ?? throw new ArgumentNullException(nameof(destinationName));

        /// <summary>
        /// Initializes a new instance of the <see cref="TravelTimeObjectiveFunction"/> class.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <exception cref="ArgumentNullException">destination</exception>
        public TravelTimeObjectiveFunction(Location destination)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            _destinationName = destination.Name;
        }

        /// <summary>
        /// Return the objective function value given a solution vector containing each decision
        /// variable. In practice, vector should be a list of parameters.
        /// </summary>
        /// <param name="arguments">Input vector of decision variables</param>
        /// <returns>Objective function value</returns>
        public double GetObjectiveValue(params StopTimeInfo[] arguments)
        {
            if (arguments.Last().StopDto.Name.TrimToLower() != _destinationName.TrimToLower())
            {
                return double.PositiveInfinity;
            }

            return (arguments.Last().DepartureTime - arguments.First().DepartureTime).TotalMinutes;
        }
    }
}