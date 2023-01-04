using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Test_2;
using Test_2.ScheduleSetup;

namespace AttendanceWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComputerController : ControllerBase
    {
        //Not sure we get classID or if we have to figure it out
        [HttpGet("{stationID}, {username}, {timeEntered}")]
        public async Task<IActionResult> LogIn(int stationID, string username, DateTime timeEntered)
        {
            (GMRClass c, Student s) session = Communicator.Current_Schedule.GetClosestClass(username, timeEntered);
            if(session.s == null)
            {
                return StatusCode(406);
            }
            var cmd =  Helper.CallStoredProcedure("dbo.StudentLoggedIn", new SqlParameter("@StudentID", session.s.ID),
                new SqlParameter("@timeSlotID", session.c.TimeSlotID),
                new SqlParameter("@TimeEntered", timeEntered),
                new SqlParameter("@StationID", stationID));
            
            await cmd.ExecuteNonQueryAsync();
            
            return Ok();
        }


        [HttpGet("{stationID}, {username}, {timeExited}")]
        public void LogOff(int stationID, string username, DateTime timeExited)
        {
            (GMRClass c, Student s) session = Communicator.Current_Schedule.GetClosestClass(username, timeExited);       
        }


        [HttpPost("{stationID}, {classID}, {time}, {applicationName}")]
        public void ApplicaitonUpdate(int stationID, string firstName, string lastName, DateTime time, string applicationName)
        {

        }
    }
}
