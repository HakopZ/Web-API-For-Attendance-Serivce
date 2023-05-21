using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Web.Http.Results;
using Test_2;
using Test_2.Filters;
using Test_2.Models;
using Test_2.ScheduleSetup;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AttendanceWebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    [EnableCors("AppPolicy")]
    public class AppController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        [HttpGet("Session/GetTimeslotInfos")]
        public ActionResult<List<Timeslot>> GetTimeslotInfos()
        {
            var x = HttpContext.User.Identity;
            List<Timeslot> mockData = new List<Timeslot>()
            {
                new Timeslot(1, new DateTime(2023, 1, 21, 22, 45, 0), new DateTime(2023, 1, 22, 0, 0, 0)),
                new Timeslot(2, new DateTime(2023, 1, 21, 0, 15, 0), new DateTime(2023, 1, 21, 1, 30, 0)),
                new Timeslot(3, new DateTime(2023, 1, 21, 1, 45, 0), new DateTime(2023, 1, 21, 4, 15, 0))
            };
            return Ok(mockData);
            //return Ok(Communicator.timeslotMap);
        }


        //[JwtAuthentication]

        [HttpGet("Session/Test")]
        public ActionResult<string> Test()
        {
            var princ = Thread.CurrentPrincipal;
            var domainName = Request.Host;
            var x = HttpContext.User.Identity;
            return Ok("Hello");
        }

        [HttpGet("Session/GetStudentInfos")]
        public ActionResult<List<Student>> GetStudentInfos()
        {
            List<Student> mockData = new List<Student>()
            {
                new Student(1, "Alice", "Allison"),
                new Student(2, "Bob", "Bobson"),
                new Student(3, "Charles", "Charleston"),
                new Student(4, "Dennis", "Denizen"),
                new Student(5, "Eddie", "Edison"),
                new Student(6, "Francis", "Franciscan"),
            };
            return Ok(mockData);
        }

        [HttpGet("Session/GetInstructorInfos")]
        public ActionResult<List<Instructor>> GetInstructorInfos()
        {
            List<Instructor> mockData = new List<Instructor>()
            {
                new Instructor(1, "Edden", "Lee"),
                new Instructor(2, "Lorenzo", "Smith"),
                new Instructor(3, "Hakop", "John"),
                new Instructor(4, "Stan", "Boss")
            };
            return Ok(mockData);
        }

        [HttpGet("Session/GetStationInfos")]
        public ActionResult<List<StationInfo>> GetStationInfos()
        {
            List<StationInfo> mockData = new List<StationInfo>()
            {
                new StationInfo(1, "118.1.1", "Room 118"),
                new StationInfo(2, "118.1.2", "Room 118"),
                new StationInfo(3, "118.1.3", "Room 118"),
                new StationInfo(4, "118.2.1", "Room 118"),
                new StationInfo(5, "119.2.2", "Room 119"),
                new StationInfo(6, "119.2.3", "Room 119"),
            };
            return Ok(mockData);
        }

        [HttpGet("Session/AllClasses")]
        public async Task<ActionResult<List<GMRSession>>> GetScheduleForTheDay()
        {
            /*
            var reader = await Helper.CallReader("GetAllClasses");

            List<GMRSession> sessions = new List<GMRSession>();
            //THIS DOES NOT WORK NEED TO FIGURE OUT INSTRUCTOR IDS
            while (await reader.ReadAsync())
            {
                var instructorReader = await Helper.CallReader("GetInstructorsWithSessionID", new SqlParameter("@SessionID", (int)reader[3]));
                List<int> instructors = new List<int>();
                while (await instructorReader.ReadAsync())
                {
                    instructors.Add((int)instructorReader[0]);
                }
                GMRSession temp = new GMRSession((int)reader[0], (int)reader[1], (int)reader[2], instructors, (StudentStatus)reader[4]);

                sessions.Add(temp);
            }*/
            List<GMRSession> mockData = new List<GMRSession>()
            {
                new GMRSession(1, 1, 1, new List<int>(){ 1,2 }, StudentStatus.Here, new DateTime(2023, 2, 19)),
                new GMRSession(2, 1, 2, new List<int>(){ 1,2 }, StudentStatus.Here, new DateTime(2023, 2, 19)),
                new GMRSession(3, 1, 3, new List<int>(){ 3 }, StudentStatus.Here, new DateTime(2023, 2, 19)),
                new GMRSession(4, 2, 4, new List<int>(){ 3 }, StudentStatus.Moved, new DateTime(2023, 2, 19)),
                new GMRSession(5, 3, 5, new List<int>(){ 4 }, StudentStatus.Here, new DateTime(2023, 2, 19)),
                new GMRSession(6, 3, 6, new List<int>(){ 4 }, StudentStatus.Here, new DateTime(2023, 2, 19)),
                new GMRSession(7, 4, 7, new List<int>(){ 5 }, StudentStatus.Here, new DateTime(2023, 2, 19))
            };
            return Ok(mockData);
        }

        [HttpGet("Session/CheckStatus")]
        public ActionResult<bool> CheckStatus()
        {
            return Ok(Communicator.SessionUpdate);
        }
        //[HttpGet("Session/GetStatus")]
        //public async Task<ActionResult<List<GMRSession>>> GetCurrentSessions()
        //{
        //    var reader = await Helper.CallReader("GetCurrentSessions");

        //    List<GMRSession> sessions = new List<GMRSession>();
        //    while (await reader.ReadAsync())
        //    {
        //        GMRSession temp = new GMRSession((int)reader[0], (int)reader[1], (int)reader[2], (int)reader[3]);
        //        sessions.Add(temp);
        //    }
        //    return Ok(sessions);
        //}



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
        public async Task<ActionResult<List<GMRSession>>> GetHistoryByDate(DateTime start, DateTime end)
        {
            var reader = await Helper.CallReader("GetSessions", new SqlParameter("@StartDate", start), new SqlParameter("@EndDate", end));

            List<GMRSession> classes = new List<GMRSession>();
            while (await reader.ReadAsync())
            {
                throw new NotImplementedException();
                // GMRSession temp = new GMRSession((int)reader[0], (int)reader[1],  (string)reader[2], (int)reader[3], (int)reader[4], (DateOnly)reader[5]);
                // classes.Add(temp);
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
                throw new NotImplementedException();
                // GMRSession temp = new GMRSession((int)reader[0], (int)reader[1], (string)reader[2], (int)reader[3], (int)reader[4], (DateOnly)reader[5]);
                // classes.Add(temp);
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
                throw new NotImplementedException();
                //GMRSession temp = new GMRSession((int)reader[0], (int)reader[1], (string)reader[2], (int)reader[3], (int)reader[4], (DateOnly)reader[5]);
                //classes.Add(temp);
            }
            return Ok(classes);
        }
    }
}
