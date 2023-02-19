using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http.Results;
using Test_2;
using Test_2.Models;
using Test_2.ScheduleSetup;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AttendanceWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppController : ControllerBase
    {
        [HttpGet("GetTimeSlotIDs")]
        public ActionResult<List<(int, DateTime, DateTime)>> GetTimeSlotIDs()
        {
            return Ok(Communicator.timeSlotMap);
        }

        [HttpGet("Session/AllClasses")]
        public async Task<ActionResult<List<GMRSession>>> GetScheduleForTheDay()
        {
            var reader = await Helper.CallReader("GetAllClasses");
            
            List<GMRSession> sessions = new List<GMRSession>();
            while (await reader.ReadAsync())
            {
                GMRSession temp = new GMRSession((int)reader[0], (int)reader[1], (int)reader[2], (int)reader[3]);
                sessions.Add(temp);
            }
            return Ok(sessions);
        }

        [HttpGet("Session/CheckStatus")]
        public ActionResult<bool> CheckStatus()
        {
            return Ok(Communicator.SessionUpdate);
        }
        [HttpGet("Session/GetStatus")]
        public async Task<ActionResult<List<GMRSession>>> GetCurrentSessions()
        {
            var reader = await Helper.CallReader("GetCurrentSessions");

            List<GMRSession> sessions = new List<GMRSession>();
            while (await reader.ReadAsync())
            {
                GMRSession temp = new GMRSession((int)reader[0], (int)reader[1], (int)reader[2], (int)reader[3]);
                sessions.Add(temp);
            }
            return Ok(sessions);
        }



        [HttpPatch("Student/UpdateStudentLocation")]
        public async Task<ActionResult> UpdateStudentLocation([FromBody] StudentLocation body)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await Helper.CallStoredProcedure("SwapSession", new SqlParameter("@OldSessionID", body.OldSessionID), new SqlParameter("@NewSessionID", body.NewSessionID),
                new SqlParameter("@InstructorID", body.InstructorID), new SqlParameter("@ReplacementID", body.ReplacementID));

            return Ok();
        }

        //public IActionResult OnLaptop()
        [HttpPatch("UpdateInstructor")]
        public async Task<ActionResult> UpdateClassInstructor([FromBody] UpdateInstructor body)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await Helper.CallStoredProcedure("SwapInstructor", new SqlParameter("@OldTeachingStationID", body.OldTeachingStationID), new SqlParameter("@NewTeachingStationID", body.NewTeachingStationID),
                new SqlParameter("@InstructorID", body.InstructorID), new SqlParameter("@ReplacementID", body.ReplacementID));

            return Ok();
        }

        [HttpGet("History/GetByDate")]
        public async Task<ActionResult<List<GMRSession>>> GetHistoryByDate([FromBody] DateTime start, [FromBody] DateTime end)
        {
            var reader = await Helper.CallReader("GetSessions", new SqlParameter("@StartDate", start), new SqlParameter("@EndDate", end));

            List<GMRSession> classes = new List<GMRSession>();
            while (await reader.ReadAsync())
            {
                GMRSession temp = new GMRSession((int)reader[0], (int)reader[1], (int)reader[3], (int)reader[4], (DateOnly)reader[5]);
                classes.Add(temp);
            }
            return Ok(classes);
        }


        [HttpGet("History/GetByStudentID")]
        public async Task<ActionResult<List<GMRSession>>> GetStudentHistory([FromBody] int studentID)
        {

            var reader = await Helper.CallReader("GetSessions", new SqlParameter("@StudentID", studentID));

            List<GMRSession> classes = new List<GMRSession>();
            while (await reader.ReadAsync())
            {
                GMRSession temp = new GMRSession((int)reader[0], (int)reader[1], (int)reader[3], (int)reader[4], (DateOnly)reader[5]);
                classes.Add(temp);
            }
            return Ok(classes);
        }

        [HttpGet("History/GetByStudentRecord")]
        public async Task<ActionResult<List<GMRClass>>> GetStudentHistoryByDate([FromBody] HistoryInfo info)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var reader = await Helper.CallReader("GetSessions", new SqlParameter("@StartDate", info.Start), new SqlParameter("@EndDate", info.End), new SqlParameter("@StudentID", info.StudentID));

            List<GMRSession> classes = new List<GMRSession>();
            while (await reader.ReadAsync())
            {
                GMRSession temp = new GMRSession((int)reader[0], (int)reader[1], (int)reader[3], (int)reader[4], (DateOnly)reader[5]);
                classes.Add(temp);
            }
            return Ok(classes);
        }
    }
}
