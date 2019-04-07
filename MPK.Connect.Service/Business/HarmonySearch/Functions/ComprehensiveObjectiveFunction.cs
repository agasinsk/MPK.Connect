using System;
using System.Linq;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Service.Utils;

namespace MPK.Connect.Service.Business.HarmonySearch.Functions
{
    /// <summary>
    /// Objective function that includes both travel time and transfer count
    /// </summary>
    public class ComprehensiveObjectiveFunction : IObjectiveFunction<StopTimeInfo>
    {
        private readonly string _destinationName;

        public ObjectiveFunctionType Type => ObjectiveFunctionType.Comprehensive;

        public ComprehensiveObjectiveFunction(string destinationName)
        {
            _destinationName = destinationName ?? throw new ArgumentNullException(nameof(destinationName));
        }

        public ComprehensiveObjectiveFunction(Location destination)
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

            var travelTime = (arguments.Last().DepartureTime - arguments.First().DepartureTime).TotalMinutes;

            var transfersPenalty = arguments.Select(s => s.Route).Distinct().Count() - 1;

            return travelTime + transfersPenalty * 10;
        }
    }
}