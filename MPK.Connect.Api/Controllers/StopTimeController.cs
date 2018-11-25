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
        [ProducesResponseType(200, Type = typeof(StopTimeDto))]
        [ProducesResponseType(400)]
        public IActionResult UpdateStopTime([FromBody] StopTimeUpdateDto stopTimeUpdateDto)
        {
            var response = _stopTimeService.UpdateStopTime(stopTimeUpdateDto);
            return response.GetActionResult();
        }

        [HttpDelete]
        [ProducesResponseType(200, Type = typeof(StopTimeDto))]
        [ProducesResponseType(400)]
        public IActionResult DeleteStopTime([FromBody] StopTimeDto stopTimeUpdateDto)
        {
            var response = _stopTimeService.DeleteStopTime(stopTimeUpdateDto);
            return response.GetActionResult();
        }
    }
}