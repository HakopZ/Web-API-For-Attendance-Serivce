using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Test_2;
using Test_2.Models;
using Test_2.ScheduleSetup;

namespace AttendanceWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComputerController : ControllerBase
    {
        //Not sure we get classID or if we have to figure it out
        Dictionary<string, int> stationLoggedInCount = new Dictionary<string, int>();
        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] MonitorInfo enterInfo)
        {
            var session = Communicator.Current_Schedule.GetClosestClass(enterInfo.AccountName, true, enterInfo.Time);
            if(session.student == null)
            {
                return NotFound();
            }
            if(stationLoggedInCount.ContainsKey(enterInfo.StationID))
            {
                stationLoggedInCount[enterInfo.StationID] += 1;
            }
            else
            {
                stationLoggedInCount.Add(enterInfo.StationID, 1);
            }

            if (stationLoggedInCount[enterInfo.StationID] > 1)
            {
                Communicator.eventMessages.Enqueue(new EventMessage(enterInfo.StationID, "Double Log In. Someone didn't log off", enterInfo.Time));
            }

            var cmd =  Helper.CallStoredProcedure("dbo.StudentLoggedIn", 
                new SqlParameter("@StudentID", session.student.ID),
                new SqlParameter("@timeSlotID", session.gmrClass.TimeSlotID),
                new SqlParameter("@TimeEntered", enterInfo.Time),
                new SqlParameter("@StationID", enterInfo.StationID));
            
            await cmd.ExecuteNonQueryAsync();
            
            return Ok();
        }


        [HttpPost("LogOff")]
        public async Task<IActionResult> LogOff([FromBody] MonitorInfo exitInfo)
        {
            (GMRClass c, Student s) session = Communicator.Current_Schedule.GetClosestClass(username, true, timeExited); 
            
            if(session.student == null)
            {
                return NotFound();
            }
            var cmd = Helper.CallStoredProcedure("dbo.StudentLoggedOff", 
                new SqlParameter("@StudentID", session.s.ID),
                new SqlParameter("@timeSlotID", session.c.TimeSlotID),
                new SqlParameter("@TimeExited", timeExited),
                new SqlParameter("@StationID", stationID));

            await cmd.ExecuteNonQueryAsync();

            return Ok();

        }


        [HttpPost("{stationID}, {classID}, {time}, {applicationName}")]
        public void ApplicaitonUpdate(int stationID, string firstName, string lastName, DateTime time, string applicationName)
        {

        }
    }
}
