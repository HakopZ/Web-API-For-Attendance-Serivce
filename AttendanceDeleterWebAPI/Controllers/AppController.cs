using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;
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
        //Getting timeSlot info
        //Test Data currently, will e getting it from Stan magic
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


        //Test code doesnt matter
        [HttpGet("Session/Test")]
        public ActionResult<string> Test()
        {
            var princ = Thread.CurrentPrincipal;
            var domainName = Request.Host;
            var x = HttpContext.User.Identity;


            return Ok("Hello");
        }

        //Test data for student info, should ping STAN or API
        //MOCK DATA
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

        //TEST DATA FOR INSTRUCTORS
        //NEEDS TO GET FROM SQL OR STAN
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

        //GET INFO OF STATIONS
        //MOCK DATA
        [HttpGet("Session/GetStationInfos")]
        public async Task<ActionResult<List<StationInfo>>> GetStationInfos()
        {
            List<StationInfo> mockData = new List<StationInfo>()
            {
                new StationInfo(1, "1", "2", "124"),
                new StationInfo(2, "1", "1", "118"),

            };

            List<StationInfo> stationInfos = new List<StationInfo>();    
            var reader = await Helper.CallReader("GetStationInfos");
            while (await reader.ReadAsync())
            {
                
                 StationInfo info = new StationInfo((int)reader[0], (string)reader[1], (string)reader[2], (string)reader[3]); 
                 stationInfos.Add(info);
            }
            return Ok(stationInfos);
        }
        private async Task<List<ScheduledClass>> GetClassesFromReader(SqlDataReader reader)
        {
            List<ScheduledClass> sessions = new List<ScheduledClass>();
            //THIS DOES NOT WORK NEED TO FIGURE OUT INSTRUCTOR IDS
            while (await reader.ReadAsync())
            {
                int sessionID = (int)reader[0];
                var classReader = await Helper.CallReader("GetClassInfo", new SqlParameter("@SessionID", sessionID));
                var instructorReader = await Helper.CallReader("GetInstructorInfo", new SqlParameter("@SessionID", sessionID));
                List<int> instructors = new List<int>();
                while (await instructorReader.ReadAsync())
                {
                    instructors.Add((int)instructorReader[0]);
                }
                ScheduledClass? temp = null;
                while (await classReader.ReadAsync())
                {
                    var returnReader = await Helper.CallReader("IsStudentAttending", new SqlParameter("@StudentID", (int)classReader[0]));
                    await returnReader.ReadAsync();
                    temp = new ScheduledClass(sessionID, (int)classReader[0], (int)reader[1], (int)reader[2], instructors, (StudentStatus)(int)returnReader[0], (DateTime)reader[5]);
                }
                if (temp == null)
                {
                    throw new InvalidDataException("FAILED to set scheduled class");
                }
                sessions.Add(temp);
            }
            return sessions;
        }
        [HttpGet("Sessions/GetScheduleForTheDay")]
        public async Task<ActionResult<List<ScheduledClass>>> GetScheduleForTheDay()
        {
            var reader = await Helper.CallReader("GetSessions", new SqlParameter("@StartDate", DateTime.UtcNow));
            return Ok(await GetClassesFromReader(reader));
        }
        //GET THE CURRENT SCHEDULE OF STUDENTS status, station, time and instructtor
        //FORMAT : {STUDENT ID, TIMSLOT ID, STATION ID, INSTRUCTORS IDs, STUDENT STATUS, DATETIME
        [HttpGet("Session/AllClasses")]
        public async Task<ActionResult<List<ScheduledClass>>> GetCurrentSessions()
        {
            
            var reader = await Helper.CallReader("GetCurrentSessions");
            var sessions = await GetClassesFromReader(reader);
            List<ScheduledClass> mockData = new List<ScheduledClass>()
            {
                new ScheduledClass(1, 1, 1, 1, new List<int>(){ 1,2 }, StudentStatus.Present, new DateTime(2023, 2, 19)),

            };
            return Ok(sessions);
        }

        //CHECK IF SCHEDULE SHOULD BE UPDATED
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


        /*
        UPDATE THE STUDENT LOCATION GIVEN
        OldSessionID 
        NewSessionID
        InstructorID
        ReplacementID
        */
        [HttpPatch("Student/UpdateStudentLocation")]
        public async Task<ActionResult> UpdateStudentLocation([FromBody] StudentLocation body)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            var reader = await Helper.CallReader("SwapSession", new SqlParameter("@OldSessionID", body.OldSessionID), new SqlParameter("@NewSessionID", body.NewSessionID),
                new SqlParameter("@InstructorID", body.InstructorID), new SqlParameter("@ReplacementID", body.ReplacementID));
            await reader.ReadAsync();

            if ((int)reader[0] == - 1)
            {
                return NotFound();
            }
            return Ok((int)reader[0]);
        }

        //Update the classes instructor 
        /*
        OldTeachingStationID 
        NewTeachingStationID 
        InstructorID 
        ReplacementID 
        */
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
        //Get Schedule history by a start date and end date
        [HttpGet("History/GetByDate")]
        public async Task<ActionResult<List<ScheduledClass>>> GetHistoryByDate(DateTime start, DateTime end)
        {
            var reader = await Helper.CallReader("GetSessions", new SqlParameter("@StartDate", start), new SqlParameter("@EndDate", end));
            var classes = await GetClassesFromReader(reader);
            return Ok(classes);
        }

        //Get All Schedule History of a Student given a student ID
        [HttpGet("History/GetByStudentID")]
        public async Task<ActionResult<List<ScheduledClass>>> GetStudentHistory([FromBody] int studentID)
        {

            var reader = await Helper.CallReader("GetSessions", new SqlParameter("@StudentID", studentID));
            var classes = await GetClassesFromReader(reader); 
            return Ok(classes);
        }

        //Get Student history based off of student id, start date and end date
        [HttpGet("History/GetByStudentRecord")]
        public async Task<ActionResult<List<GMRClass>>> GetStudentHistoryByDate([FromBody] HistoryInfo info)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var reader = await Helper.CallReader("GetSessions", new SqlParameter("@StartDate", info.Start), new SqlParameter("@EndDate", info.End), new SqlParameter("@StudentID", info.StudentID));

            var classes = await GetClassesFromReader(reader);
            return Ok(classes);
        }
    }
}
