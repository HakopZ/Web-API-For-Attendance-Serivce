using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Linq;
using Test_2;
using Test_2.FilterClasses;
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
            
            await Communicator.sqlConnection.OpenAsync();
            SqlCommand cmd = new SqlCommand("GetAllClasses");
            var reader = await cmd.ExecuteReaderAsync();
            List<GMRSession> sessions = new List<GMRSession>();
            while(await reader.ReadAsync())
            {
                GMRSession temp = new GMRSession() { StationID = (int)reader[0], StudentID = (int)reader[1], TimeSlotID = (int)reader[2] };
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
        public async Task<ActionResult<List<GMRSession>>> GetCurrentStatus()
        {
            await Communicator.sqlConnection.OpenAsync();
            SqlCommand cmd = new SqlCommand("GetCurrentSessions");
            var reader = await cmd.ExecuteReaderAsync();
            List<GMRSession> sessions = new List<GMRSession>();
            while(await reader.ReadAsync())
            {
                GMRSession temp = new GMRSession() { StationID = (int)reader[0], StudentID = (int)reader[1], TimeSlotID = (int)reader[2] };
                sessions.Add(temp);
            }
            return Ok(sessions);
        }



        [HttpPatch("Student/UpdateStudentLocation")]
        public ActionResult UpdateStudentLocation([FromBody] StudentLocation body)
        {
            throw new NotImplementedException();
        }

        //public IActionResult OnLaptop()
        [HttpPatch("UpdateInstructor")]
        public ActionResult UpdateClassInstructor([FromBody] UpdateInstructor body)
        {
            throw new NotImplementedException();
        }

        [HttpGet("History/GetByDate")]
        public ActionResult<List<GMRClass>> GetHistoryByDate([FromBody] DateTime start, [FromBody] DateTime end)
        {
            throw new NotImplementedException();
        }


        [HttpGet("History/GetByStudentID")]
        public ActionResult<List<GMRClass>> GetStudentHistory([FromBody] int studentID)
        {
            throw new NotImplementedException();
        }

        [HttpGet("History/GetByStudentRecord")]
        public ActionResult<List<GMRClass>> GetStudentHistoryByDate([FromBody] HistoryInfo info)
        {
            throw new NotImplementedException();
        }
    }
}
