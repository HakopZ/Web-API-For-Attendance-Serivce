using AttendanceWebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Test_2.Models;
using Test_2.ScheduleSetup;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Test_2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MagicController : ControllerBase
    {
        [HttpPost("MakeSchedule")]
        public void MakeSchedule([FromBody] GMRSchedule schedule)
        {
            Communicator.Current_Schedule = schedule;
        }

        [HttpPatch("UpdateSchedule")]
        public Task<ActionResult> UpdateSchedule([FromBody] UpdateScheduleInfo info)
        {
            var cmd = Helper.CallStoredProcedure("UpdateSchedule", new SqlParameter("@StudentID", ));
        }
        [HttpPost("MakeTimeSlotMapper")]
        public void MakeTimeSlotMapper(List<(int, DateTime, DateTime)> values)
        {
            Communicator.timeSlotMap = values;
        }

        //constantly running task that checks for schedule changes
    }
}
