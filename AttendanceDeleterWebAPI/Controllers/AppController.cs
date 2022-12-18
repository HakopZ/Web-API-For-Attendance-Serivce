using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Test_2.Models;
using Test_2.ScheduleSetup;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AttendanceWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppController : ControllerBase
    {
        [HttpGet("Session/CheckStatus")]
        public bool CheckStatus()
        {
            throw new NotImplementedException();
        }
        [HttpGet("Session/GetStatus")]
        public List<GMRClass> GetStatus()
        {
            throw new NotImplementedException();
        }

        [HttpPatch("Student/Attendance")]
        public void UpdateStudentAttendance([FromBody] StudentAttendance body)
        {
            throw new NotImplementedException();
        }

        [HttpPatch("Student/Location")]
        public void UpdateStudentLocation([FromBody] StudentLocation body)
        {
            throw new NotImplementedException();
        }

        [HttpPatch("Instructor")]
        public void UpdateClassInstructor([FromBody] UpdateInstructor body)
        {
            throw new NotImplementedException();
        }

        [HttpGet("History/ByDate")]
        public IActionResult GetHistoryByDate([FromQuery] TimeRange body)
        {
            throw new NotImplementedException();
        }
        

        [HttpGet("History/ByID")]
        public IActionResult GetStudentHistory([FromQuery] StudentInfo body)
        {
            throw new NotImplementedException();
        }

        [HttpGet("History/ByStudentRecord")]
        public IActionResult GetStudentHistoryByDate([FromQuery] StudentRecord body)
        {
            throw new NotImplementedException();
        }
    }
}
