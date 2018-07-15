using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess.Routes.Types;
using MPK.Connect.Model;

namespace MPK.Connect.Service.Service.Routes.Types
{
    public class RouteTypeService : IGenericService<RouteType>
    {
        private readonly ILogger<RouteTypeService> _logger;
        private readonly IRouteTypeRepository _routeTypeRepository;

        public RouteTypeService(IRouteTypeRepository routeRepository, ILogger<RouteTypeService> logger)
        {
            _routeTypeRepository = routeRepository ?? throw new ArgumentNullException(nameof(routeRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public int ReadFromFile(string filePath)
        {
            var routeTypes = new List<RouteType>();
            var totalCount = 0;

            using (var streamReader = new StreamReader(filePath))
            {
                var routeTypeString = streamReader.ReadLine();

                while ((routeTypeString = streamReader.ReadLine()) != null)
                {
                    var mappedRouteType = MapToRoute(routeTypeString);
                    routeTypes.Add(mappedRouteType);
                }
                _logger.LogInformation($"Read {routeTypes.Count} route types.");
                _logger.LogInformation("Sorting route types by RouteId...");
                routeTypes.Sort((route1, route2) => route1.Id.CompareTo(route2.Id));
                _logger.LogInformation("Route types have been sorted!");

                _routeTypeRepository.AddRange(routeTypes);
                _routeTypeRepository.Save();
                totalCount = routeTypes.Count;
                routeTypes.Clear();
                _logger.LogInformation("Route types have been successfully saved!");
            }

            return totalCount;
        }

        private RouteType MapToRoute(string routeString)
        {
            var routeTypeInfos = routeString.Split(',');
            var id = int.Parse(routeTypeInfos[0]);
            var name = routeTypeInfos[1];
            var routeType = new RouteType
            {
                RouteTypeId = id,
                Name = name
            };
            return routeType;
        }
    }
}