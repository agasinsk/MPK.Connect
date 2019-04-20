using Microsoft.AspNetCore.Mvc;
using MPK.Connect.Model.Business;
using MPK.Connect.Service.Business;

namespace MPK.Connect.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeTableController : ControllerBase
    {
        private readonly ITimeTableService _timeTableService;

        public TimeTableController(ITimeTableService timeTableService)
        {
            _timeTableService = timeTableService;
        }

        [HttpGet("{stopId:int}")]
        public TimeTable Get(int stopId)
        {
            return _timeTableService.GetTimeTable(stopId);
        }
    }
}