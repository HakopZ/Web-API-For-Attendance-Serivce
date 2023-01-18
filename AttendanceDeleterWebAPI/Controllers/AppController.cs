using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("Session/CheckStatus")]
        public bool CheckStatus()
        {
            return Communicator.EventOccured;
        }
        [HttpGet("Session/GetStatus")]
        public IActionResult GetStatus([FromBody] DateTime time)
        {
            List<Student> students = new List<Student>();
            StatusInfo statusInfo;
            foreach(var clss in Communicator.Current_Schedule.Classes)
            {
                var stds = clss.Students.Where(x => x.Attended);
                if(stds.Count > 0)
                {
                    students.AddRange(stds);
                }
            }

            statusInfo.studentsLoggedIn = students;
            statusInfo.classes = new List<GMRClass>();
            int timeSlot = time.ToTimeSlot();
            
            if(timeSlot != -1)
            {
                var currentClasses = Communicator.Current_Schedule.GetClassesByTime(timeSlot);
                if (currentClasses.Count == 0)
                {
                    return NotFound();
                }
                statusInfo.classes = currentClasses;
                return Ok(statusInfo);
            }
            return Ok(statusInfo);
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
