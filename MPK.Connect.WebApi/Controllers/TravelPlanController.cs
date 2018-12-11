using Microsoft.AspNetCore.Mvc;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Service.Business;
using System;

namespace MPK.Connect.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelPlanController : ControllerBase
    {
        private readonly ITravelPlanService _travelPlanService;

        public TravelPlanController(ITravelPlanService travelPlanService)
        {
            _travelPlanService = travelPlanService ?? throw new ArgumentNullException(nameof(travelPlanService));
        }

        [HttpGet]
        public string Get([FromBody] TravelLocations travelLocations)
        {
            return _travelPlanService.GetTravelPlan(travelLocations.Source, travelLocations.Destination);
        }
    }
}