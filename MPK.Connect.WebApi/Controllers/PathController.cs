using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MPK.Connect.Service.Business.Graph;

namespace MPK.Connect.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PathController : ControllerBase
    {
        private readonly IStopMapManager _stopMapManager;

        public PathController(IStopMapManager stopMapManager)
        {
            _stopMapManager = stopMapManager ?? throw new ArgumentNullException(nameof(stopMapManager));
        }

        // GET: api/Path
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _stopMapManager.InitializeGraph();
        }
    }
}