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

            Communicator.EventOccured = true;
            return Ok();
        }


        [HttpPost("LogOff")]
        public async Task<IActionResult> LogOff([FromBody] MonitorInfo exitInfo)
        {
            var session = Communicator.Current_Schedule.GetClosestClass(exitInfo.AccountName, false, exitInfo.Time); 
            
            if(session.student == null)
            {
                return NotFound();
            }
            var cmd = Helper.CallStoredProcedure("dbo.StudentLoggedOff", 
                new SqlParameter("@StudentID", session.student.ID),
                new SqlParameter("@timeSlotID", session.gmrClass.TimeSlotID),
                new SqlParameter("@TimeExited", exitInfo.Time),
                new SqlParameter("@StationID", exitInfo.StationID));

            await cmd.ExecuteNonQueryAsync();
            Communicator.EventOccured = true;
            return Ok();

        }


        [HttpPost("ApplicationUpdate")]
        public IActionResult ApplicaitonUpdate([FromBody] MonitorInfo studentInfo)
        {
            var session = Communicator.Current_Schedule.GetClosestClass(studentInfo.AccountName, true, studentInfo.Time);
            if(session.student == null)
            {
                return NotFound();
            }
            session.student.CurrentApp = studentInfo.ForegroundWindowTitle;

            Communicator.EventOccured = true;

            return Ok();
        }
    }
}
