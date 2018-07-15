using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess.Routes;
using MPK.Connect.Model;

namespace MPK.Connect.Service.Service.Routes
{
    public class RouteService : IRouteService
    {
        private readonly ILogger<RouteService> _logger;
        private readonly IRouteRepository _routeRepository;

        public RouteService(IRouteRepository routeRepository, ILogger<RouteService> logger)
        {
            _routeRepository = routeRepository ?? throw new ArgumentNullException(nameof(routeRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public int ReadRoutesFromFile(string filePath)
        {
            var allRoutes = new List<Route>();
            var allRoutesCount = 0;

            using (var streamReader = new StreamReader(filePath))
            {
                var routeString = streamReader.ReadLine();

                while ((routeString = streamReader.ReadLine()) != null)
                {
                    var createdStop = MapToRoute(routeString);
                    allRoutes.Add(createdStop);
                }
                _logger.LogInformation($"Read {allRoutes.Count} routes.");
                _logger.LogInformation("Sorting routes by RouteId...");
                allRoutes.Sort((route1, route2) => route1.Id.CompareTo(route2.Id));
                _logger.LogInformation("Routes have been sorted!");

                _routeRepository.AddRange(allRoutes);
                allRoutesCount = allRoutes.Count;
                allRoutes.Clear();
                _logger.LogInformation("Routes have been successfully saved!");
            }

            return allRoutesCount;
        }

        private Route MapToRoute(string routeString)
        {
            var routeInfos = routeString.Split(',');

            var mappedRoute = new Route
            {
                Id = routeInfos[0],
                AgencyId = Int32.Parse(routeInfos[1]),
                ShortName = routeInfos[2].Replace("\"", "").Trim(),
                LongName = routeInfos[3].Replace("\"", ""),
                Description = routeInfos[4].Replace("\"", ""),
                ValidFrom = DateTime.Parse(routeInfos[7].Replace("\"", "")),
                ValidUntil = DateTime.Parse(routeInfos[8].Replace("\"", "")),
            };
            return mappedRoute;
        }
    }
}