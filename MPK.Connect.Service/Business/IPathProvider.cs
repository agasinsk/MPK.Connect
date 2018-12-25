﻿using System.Collections.Generic;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Model.Graph;

namespace MPK.Connect.Service.Business
{
    public interface IPathProvider
    {
        List<Path<StopTimeInfo>> GetAvailablePaths(Graph<string, StopTimeInfo> graph, Location sourceLocation,
            Location destinationLocation);
    }
}