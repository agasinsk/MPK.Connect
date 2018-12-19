using System;
using System.Linq;
using MPK.Connect.DataAccess;
using MPK.Connect.Model;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;

namespace MPK.Connect.Service.Business
{
    public class CoordinateLimitsProvider : ICoordinateLimitsProvider
    {
        private readonly IGenericRepository<Stop> _stopRepository;
        private readonly double _defaultLimitOverhead = 0.1;

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

        private CoordinateLimits GetLimitsFromStopCoordinates(Location sourceLocation, Location destinationLocation)
        {
            if (string.IsNullOrEmpty(sourceLocation.Name) || string.IsNullOrEmpty(destinationLocation.Name))
            {
                return null;
            }

            var sourceStop = _stopRepository.GetAll()
                .FirstOrDefault(s => s.Name.Trim().ToLower() == sourceLocation.Name.Trim().ToLower());

            var destinationStop = _stopRepository.GetAll()
                .FirstOrDefault(s => s.Name.Trim().ToLower() == destinationLocation.Name.Trim().ToLower());

            if (sourceStop != null && destinationStop != null)
            {
                return GetLimitsDirectlyFromCoordinates(sourceStop.Latitude, sourceStop.Longitude,
                    destinationStop.Latitude, destinationStop.Longitude);
            }

            return null;
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

        private bool CheckIfCoordinatesAreAvailable(Location sourceLocation, Location destinationLocation)
        {
            return sourceLocation.Latitude.HasValue && sourceLocation.Longitude.HasValue &&
                            destinationLocation.Latitude.HasValue && destinationLocation.Longitude.HasValue;
        }
    }
}