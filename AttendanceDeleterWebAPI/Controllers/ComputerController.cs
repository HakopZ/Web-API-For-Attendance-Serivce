﻿using Microsoft.AspNetCore.Mvc;
using System.Data;
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
            //var session = Communicator.Current_Schedule.GetClosestClassWithStudent(enterInfo.AccountName, true);
            //if(session.student == null)
            //{
            //    return NotFound();
            //}
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
                Communicator.eventMessages.Enqueue(new EventMessage(enterInfo.StationID, "Double Log In. Someone didn't log off", TimeOnly.FromDateTime(DateTime.Now)));
            }

            //if(!session.student.LogIn(enterInfo.StationID))
            //{
            //    var std = session.gmrClass.Students.Where(x => x.StationID == enterInfo.StationID).First();
            //    string station = session.student.StationID;
            //    session.student.StationID = enterInfo.StationID;
            //    std.StationID = station;
            //}

            //var cmd = await Helper.CallStoredProcedure("dbo.StudentLogIn",
            //    new SqlParameter("@StudentID", session.student.ID),
            //    new SqlParameter("@timeSlotID", session.gmrClass.TimeSlotID),
            //    new SqlParameter("@IsManual", false),
            //    new SqlParameter("@StationID", enterInfo.StationID));
            
            //var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            //returnParameter.Direction = ParameterDirection.ReturnValue;
            //await cmd.ExecuteNonQueryAsync();
            //await Communicator.sqlConnection.CloseAsync();
            //Communicator.Current_Schedule.Updated = true;
            //session.student.Entered = (DateTime)returnParameter.Value;
            return Ok();
        }


        [HttpPost("LogOff")]
        public async Task<IActionResult> LogOff([FromBody] MonitorInfo exitInfo)
        {
            var session = Communicator.Current_Schedule.GetClosestClassWithStudent(exitInfo.AccountName, true); 
            
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
                eventLog.LogInformation("Log Off When There wasn't a log in", (exitInfo.StationID));
            }

            session.student.LogOut();

            var cmd = Helper.CallStoredProcedure("dbo.StudentLogOff", 
                new SqlParameter("@StudentID", session.student.ID),
                new SqlParameter("@timeSlotID", session.gmrClass.TimeSlotID),
                new SqlParameter("@StationID", exitInfo.StationID));

            var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;
            await cmd.ExecuteNonQueryAsync();

            Communicator.Current_Schedule.Updated = true;
            session.student.Exited = (DateTime)returnParameter.Value;
            return Ok();

        }


        [HttpPost("{stationID}, {classID}, {time}, {applicationName}")]
        public IActionResult ApplicaitonUpdate([FromBody] MonitorInfo monitorInfo)
        {
            var session = Communicator.Current_Schedule.GetClosestClassWithStudent(monitorInfo.AccountName, true);
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
