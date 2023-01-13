﻿using Microsoft.AspNetCore.Mvc;
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
            (GMRClass c, Student s) session = Communicator.Current_Schedule.GetClosestClass(username, true, timeEntered);
            if(session.s == null)
            {
                return StatusCode(406);
            }
            /*var cmd =  Helper.CallStoredProcedure("dbo.StudentLoggedIn", 
                new SqlParameter("@StudentID", session.s.ID),
                new SqlParameter("@timeSlotID", session.c.TimeSlotID),
                new SqlParameter("@TimeEntered", timeEntered),
                new SqlParameter("@StationID", stationID));
            
            await cmd.ExecuteNonQueryAsync();*/
            Communicator.Current_Schedule.Updated = true;
            return Ok();
        }


        [HttpGet("{stationID}, {username}, {timeExited}")]
        public async Task<IActionResult> LogOff(int stationID, string username, DateTime timeExited)
        {
            (GMRClass gmrClass, Student student) session = Communicator.Current_Schedule.GetClosestClass(username, true, timeExited); 
            
            if(session.student == null)
            {
                return StatusCode(406);
            }
            var cmd = Helper.CallStoredProcedure("dbo.StudentLoggedOff", 
                new SqlParameter("@StudentID", session.student.ID),
                new SqlParameter("@timeSlotID", session.gmrClass.TimeSlotID),
                new SqlParameter("@TimeExited", timeExited),
                new SqlParameter("@StationID", stationID));

            await cmd.ExecuteNonQueryAsync();

            Communicator.Current_Schedule.Updated = true;
            return Ok();

        }


        [HttpPost("{stationID}, {classID}, {time}, {applicationName}")]
        public void ApplicaitonUpdate(int stationID, string username, DateTime time, string applicationName)
        {
            var session = Communicator.Current_Schedule.GetClosestClass(username, true, time);
            session.student.CurrentApp = applicationName;


            Communicator.Current_Schedule.Updated = true;
        }
    }
}
