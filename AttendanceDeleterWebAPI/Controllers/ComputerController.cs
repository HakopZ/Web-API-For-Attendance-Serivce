using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Linq;
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
        ILogger<string> eventLog = new Logger<string>(new LoggerFactory());
        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] MonitorInfo enterInfo)
        {
            var session = Communicator.Current_Schedule.GetClosestClassWithStudent(enterInfo.AccountName, true, enterInfo.Time);
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

            if(!session.student.LogIn(enterInfo.Time, enterInfo.StationID))
            {
                var std = session.gmrClass.Students.Where(x => x.StationID == enterInfo.StationID).First();
                string station = session.student.StationID;
                session.student.StationID = enterInfo.StationID;
                std.StationID = station;
            }

            var cmd =  Helper.CallStoredProcedure("dbo.StudentLoggedIn", 
                new SqlParameter("@StudentID", session.student.ID),
                new SqlParameter("@timeSlotID", session.gmrClass.TimeSlotID),
                new SqlParameter("@TimeEntered", enterInfo.Time),
                new SqlParameter("@StationID", enterInfo.StationID));
            
            await cmd.ExecuteNonQueryAsync();

            Communicator.Current_Schedule.Updated = true;
            return Ok();
        }


        [HttpPost("LogOff")]
        public async Task<IActionResult> LogOff([FromBody] MonitorInfo exitInfo)
        {
            var session = Communicator.Current_Schedule.GetClosestClassWithStudent(exitInfo.AccountName, true, exitInfo.Time); 
            
            if(session.student == null)
            {
                return NotFound();
            }

            if(stationLoggedInCount.ContainsKey(exitInfo.StationID))
            {
                stationLoggedInCount[exitInfo.StationID]--;
            }
            else
            {
                eventLog.LogInformation("Log Off When There wasn't a log in", (exitInfo.StationID, exitInfo.Time));
            }

            session.student.LogOut(exitInfo.Time);

            var cmd = Helper.CallStoredProcedure("dbo.StudentLoggedOff", 
                new SqlParameter("@StudentID", session.student.ID),
                new SqlParameter("@timeSlotID", session.gmrClass.TimeSlotID),
                new SqlParameter("@TimeExited", exitInfo.Time),
                new SqlParameter("@StationID", exitInfo.StationID));

            await cmd.ExecuteNonQueryAsync();

            Communicator.Current_Schedule.Updated = true;
            return Ok();

        }


        [HttpPost("{stationID}, {classID}, {time}, {applicationName}")]
        public IActionResult ApplicaitonUpdate([FromBody] MonitorInfo monitorInfo)
        {
            var session = Communicator.Current_Schedule.GetClosestClassWithStudent(monitorInfo.AccountName, true, monitorInfo.Time);
            if(session.student == null)
            {
                return NotFound();
            }
            session.student.CurrentApp = monitorInfo.ForegroundWindowTitle;
            session.student.CurrentFileName = monitorInfo.CurrentFileName;

            Communicator.Current_Schedule.Updated = true;
            return Ok();
        }
    }
}
