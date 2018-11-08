using Microsoft.AspNetCore.Mvc;
using MPK.Connect.Model;
using MPK.Connect.Service.Business;
using System;
using System.Collections.Generic;

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
        public List<Stop> GetAll()
        {
            return _stopService.GetAllStops();
        }

        [HttpGet("[action]")]
        public Stop GetById(string stopId)
        {
            return _stopService.GetStopById(stopId);
        }

        [HttpGet("[action]")]
        public List<Stop> GetByName(string stopName)
        {
            return _stopService.GetStopByName(stopName);
        }
    }
}