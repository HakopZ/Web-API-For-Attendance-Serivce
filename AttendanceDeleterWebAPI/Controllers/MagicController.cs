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
        public async void MakeSchedule([FromBody] List<GMRSession> schedule)
        {
            
            foreach(var session in schedule)
            {
                await Helper.CallStoredProcedure("CreateSession", new SqlParameter("@StudentID", session.StationID), new SqlParameter("@TimeSlotID", session.TimeSlotID),
                           new SqlParameter("@StationID", session.StationID), new SqlParameter("@InstructorID", session.InstructorID));
            }
        }

        [HttpPatch("UpdateSchedule")]
        public async Task<ActionResult> UpdateSchedule([FromBody] UpdateScheduleInfo info)
        {
            if (info.NewTimeSlotID == null)
            {
                if (info.InstructorID == null)
                {
                    await Helper.CallStoredProcedure("CancelSession", new SqlParameter("@StudentID", info.StudentID), new SqlParameter("@TimeSlotID", info.TimeSlotID));
                }
                else
                {
                    await Helper.CallStoredProcedure("CreateSession", new SqlParameter("@StudentID", info.StudentID), new SqlParameter("@TimeSlotID", info.TimeSlotID),
                        new SqlParameter("@StationID", info.StationID), new SqlParameter("@InstructorID", info.InstructorID));
                }
            }
            else
            {
                await Helper.CallStoredProcedure("MoveSession", new SqlParameter("@StudentID", info.StudentID), new SqlParameter("@OldTimeSlotID", info.TimeSlotID),
                        new SqlParameter("@NewTimeSlotID", info.NewTimeSlotID), new SqlParameter("@StationID", info.StationID), new SqlParameter("@InstructorID", info.InstructorID));
            }
            Communicator.SessionUpdate = true;
            return Ok();
        }
        [HttpPost("MakeTimeSlotMapper")]
        public void MakeTimeSlotMapper(List<(int, DateTime, DateTime)> values)
        {
            Communicator.timeSlotMap = values;
        }

        //constantly running task that checks for schedule changes
    }
}
