using System;
using Microsoft.AspNetCore.Mvc;
using MPK.Connect.Model.Business;
using MPK.Connect.Service.Business;

namespace MPK.Connect.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StopTimeController : ControllerBase
    {
        private readonly IStopTimeService _stopTimeService;

        public StopTimeController(IStopTimeService stopTimeService)
        {
            _stopTimeService = stopTimeService ?? throw new ArgumentNullException(nameof(stopTimeService));
        }

        [HttpPut]
        public StopTimeDto UpdateStopTime([FromBody] StopTimeUpdateInfo stopTimeUpdateInfo)
        {
            return _stopTimeService.UpdateStopTime(stopTimeUpdateInfo);
        }

        [HttpDelete]
        public StopTimeDto DeleteStopTime([FromBody] StopTimeInfo stopTimeUpdateInfo)
        {
            return _stopTimeService.DeleteStopTime(stopTimeUpdateInfo);
        }
    }
}