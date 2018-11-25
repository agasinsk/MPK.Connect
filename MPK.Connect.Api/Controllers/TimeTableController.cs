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

        // GET: api/TimeTable/5
        [HttpGet("{stopId}", Name = "Get")]
        public TimeTable Get(string stopId)
        {
            return _timeTableService.GetTimeTable(stopId);
        }

        // POST: api/TimeTable
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/TimeTable/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}