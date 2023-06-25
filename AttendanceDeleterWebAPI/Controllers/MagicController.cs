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

        //Getting schedule from STAN
        [HttpPost("MakeSchedule")]
        public async void MakeSchedule([FromBody] List<GMRSession> schedule)
        {
            
            foreach(var session in schedule)
            {
                await Helper.CallStoredProcedure("CreateSession", new SqlParameter("@StudentID", session.StationID), new SqlParameter("@TimeslotID", session.TimeslotID),
                           new SqlParameter("@StationID", session.StationID), new SqlParameter("@InstructorID", session.InstructorIDs));
            }
        }

        //UPDATING SCHEDULE FROM STAN

        [HttpPatch("UpdateSchedule")]
        public async Task<ActionResult> UpdateSchedule([FromBody] UpdateScheduleInfo info)
        {
            if (info.NewTimeslotID == null)
            {
                if (info.InstructorID == null)
                {
                    await Helper.CallStoredProcedure("CancelSession", new SqlParameter("@StudentID", info.StudentID), new SqlParameter("@TimeslotID", info.TimeslotID));
                }
                else
                {
                    await Helper.CallStoredProcedure("CreateSession", new SqlParameter("@StudentID", info.StudentID), new SqlParameter("@TimeslotID", info.TimeslotID),
                        new SqlParameter("@StationID", info.StationID), new SqlParameter("@InstructorID", info.InstructorID));
                }
            }
            else
            {
                await Helper.CallStoredProcedure("MoveSession", new SqlParameter("@StudentID", info.StudentID), new SqlParameter("@OldTimeslotID", info.TimeslotID),
                        new SqlParameter("@NewTimeslotID", info.NewTimeslotID), new SqlParameter("@StationID", info.StationID), new SqlParameter("@InstructorID", info.InstructorID));
            }
            Communicator.SessionUpdate = true;
            return Ok();
        }

        //UPDATE NEW TIMESLOTS
        [HttpPost("MakeTimeslotMapper")]
        public void MakeTimeslotMapper([FromBody] List<Timeslot> values)
        {
            Communicator.timeslotMap = values;
        }

        //GET STUDENT IDs from name
        [HttpPost("GetStudentID")]
        public async void GetStudentID([FromBody] Name name)
        {
            await Helper.CallStoredProcedure("GetStudentID", new SqlParameter("@Firstname", name.FirstName), new SqlParameter("@Lastname", name.LastName));
        }

        //constantly running task that checks for schedule changes
    }
}
