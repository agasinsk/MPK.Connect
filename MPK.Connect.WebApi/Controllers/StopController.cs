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
    public class StopController : ControllerBase
    {
        private readonly IStopService _stopService;

        public StopController(IStopService stopService)
        {
            _stopService = stopService ?? throw new ArgumentNullException(nameof(stopService));
        }

        [HttpGet("[action]")]
        public List<StopDto> GetNamed()
        {
            return _stopService.GetDistinctStopsByName();
        }

        [HttpGet("[action]")]
        public List<StopDto> GetAll()
        {
            return _stopService.GetAllStops();
        }

        [HttpGet("[action]")]
        public StopDto GetById(string stopId)
        {
            return _stopService.GetStopById(stopId);
        }

        [HttpGet("[action]")]
        public List<StopDto> GetByName(string stopName)
        {
            return _stopService.GetStopByName(stopName);
        }
    }
}