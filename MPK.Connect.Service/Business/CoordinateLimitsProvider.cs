using System;
using System.Linq;
using MPK.Connect.DataAccess;
using MPK.Connect.Model;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Service.Utils;

namespace MPK.Connect.Service.Business
{
    public class CoordinateLimitsProvider : ICoordinateLimitsProvider
    {
        private readonly double _defaultLimitOverhead = 0.1;
        private readonly IGenericRepository<Stop> _stopRepository;

        public CoordinateLimitsProvider(IGenericRepository<Stop> stopRepository)
        {
            _stopRepository = stopRepository ?? throw new ArgumentNullException(nameof(stopRepository));
        }

        public CoordinateLimits GetCoordinateLimits(Location sourceLocation, Location destinationLocation)
        {
            if (CheckIfCoordinatesAreAvailable(sourceLocation, destinationLocation))
            {
                return GetLimitsDirectlyFromCoordinates(sourceLocation.Latitude.Value, sourceLocation.Longitude.Value, destinationLocation.Latitude.Value, destinationLocation.Longitude.Value);
            }

            return GetLimitsFromStopCoordinates(sourceLocation, destinationLocation);
        }

        private bool CheckIfCoordinatesAreAvailable(Location sourceLocation, Location destinationLocation)
        {
            return sourceLocation.Latitude.HasValue && sourceLocation.Longitude.HasValue &&
                            destinationLocation.Latitude.HasValue && destinationLocation.Longitude.HasValue;
        }

        private CoordinateLimits GetLimitsDirectlyFromCoordinates(double sourceLocationLatitude, double sourceLocationLongitude, double destinationLocationLatitude, double destinationLocationLongitude)
        {
            var limits = new CoordinateLimits();

            // set latitude
            if (sourceLocationLatitude > destinationLocationLatitude)
            {
                limits.UpperLeftLatitude = sourceLocationLatitude;
                limits.LowerRightLatitude = destinationLocationLatitude;
            }
            else
            {
                limits.UpperLeftLatitude = destinationLocationLatitude;
                limits.LowerRightLatitude = sourceLocationLatitude;
            }

            // set longitude
            if (sourceLocationLongitude < destinationLocationLongitude)
            {
                limits.UpperLeftLongitude = sourceLocationLongitude;
                limits.LowerRightLongitude = destinationLocationLongitude;
            }
            else
            {
                limits.UpperLeftLongitude = destinationLocationLongitude;
                limits.LowerRightLongitude = sourceLocationLongitude;
            }
            limits.Add(_defaultLimitOverhead);

            return limits;
        }

        private CoordinateLimits GetLimitsFromStopCoordinates(Location sourceLocation, Location destinationLocation)
        {
            if (string.IsNullOrEmpty(sourceLocation.Name) || string.IsNullOrEmpty(destinationLocation.Name))
            {
                return null;
            }

            var sourceStop = _stopRepository.GetAll()
                .FirstOrDefault(s => s.Name.TrimToLower() == sourceLocation.Name.TrimToLower());

            var destinationStop = _stopRepository.GetAll()
                .FirstOrDefault(s => s.Name.TrimToLower() == destinationLocation.Name.TrimToLower());

            if (sourceStop != null && destinationStop != null)
            {
                return GetLimitsDirectlyFromCoordinates(sourceStop.Latitude, sourceStop.Longitude,
                    destinationStop.Latitude, destinationStop.Longitude);
            }

            return null;
        }
    }
}