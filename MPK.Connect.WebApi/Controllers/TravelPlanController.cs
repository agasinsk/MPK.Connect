using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Service.Business;

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

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TravelPlan>))]
        [ProducesResponseType(400)]
        public IEnumerable<TravelPlan> Get([FromBody] TravelOptions travelOptions)
        {
            return _travelPlanService.GetTravelPlans(travelOptions);
        }
    }
}