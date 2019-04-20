using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MPK.Connect.Model;
using MPK.Connect.Model.Business;
using MPK.Connect.Service.Business;

namespace MPK.Connect.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StopsController : ControllerBase
    {
        private readonly IStopService _stopService;

        public StopsController(IStopService stopService)
        {
            _stopService = stopService ?? throw new ArgumentNullException(nameof(stopService));
        }

        [HttpGet]
        public List<StopDto> GetAll()
        {
            return _stopService.GetAllStops();
        }

        [HttpGet("{stopId:int}")]
        public StopDto GetById(int stopId)
        {
            return _stopService.GetStopById(stopId);
        }

        [HttpGet("{stopName}")]
        public List<StopDto> GetByName(string stopName)
        {
            return _stopService.GetStopByName(stopName);
        }

        [HttpGet("Named")]
        public List<StopDto> GetNamed()
        {
            return _stopService.GetDistinctStopsByName();
        }
    }
}