using System;
using System.Linq;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Service.Utils;

namespace MPK.Connect.Service.Business.HarmonySearch.Functions
{
    /// <summary>
    /// Objective function that includes transfer count only
    /// </summary>
    public class TransfersObjectiveFunction : IObjectiveFunction<StopTimeInfo>
    {
        private readonly string _destinationName;

        public ObjectiveFunctionType Type => ObjectiveFunctionType.Transfers;

        public TransfersObjectiveFunction(string destinationName)
        {
            _destinationName = destinationName ?? throw new ArgumentNullException(nameof(destinationName));
        }

        public TransfersObjectiveFunction(Location destination)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            _destinationName = destination.Name;
        }

        public double GetObjectiveValue(params StopTimeInfo[] arguments)
        {
            if (arguments.Last().StopDto.Name.TrimToLower() != _destinationName.TrimToLower())
            {
                return double.PositiveInfinity;
            }

            return arguments.Select(s => s.Route).Distinct().Count() - 1;
        }
    }
}