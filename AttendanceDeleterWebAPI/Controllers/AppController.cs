using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("AllClasses")]
        public ActionResult<GMRClass> GetAllClasses()
        {
            return Ok(Communicator.Current_Schedule.Classes);
        }
        [HttpGet("Session/CheckStatus")]
        public ActionResult<bool> CheckStatus()
        {
            return Ok(Communicator.Current_Schedule.Updated);
        }
        [HttpGet("Session/GetStatus")]
        public ActionResult<StatusInfo> GetStatus([FromBody] DateTime time)
        {
            List<Student> students = new List<Student>();
            StatusInfo statusInfo;

            foreach (var clss in Communicator.Current_Schedule.Classes)
            {
                var stds = clss.Students.Where(x => x.Attended);
                if(stds.Count() > 0)
                {
                    students.AddRange(stds);
                }
            }
            
            int timeSlot = time.ToTimeSlot();
            
            if(timeSlot != -1)
            {
                
                var currentClasses = Communicator.Current_Schedule.FilterForClass(new List<IFilter>() { new TimeSlotFilter(timeSlot) });
                if (currentClasses.Count == 0)
                {
                    return NotFound();
                }
                statusInfo = new StatusInfo(students, currentClasses);
                return Ok(statusInfo);
            }
            statusInfo = new StatusInfo(students, new List<GMRClass>());
            return Ok(statusInfo);
        }

        

        [HttpPatch("Student/Location")]
        public ActionResult UpdateStudentLocation([FromBody] StudentLocation body)
        {
            List<IFilter> filters = new List<IFilter>()
            {
                new TimeSlotFilter(body.TimeSlotID),
                new StudentFilter(body.StudentID)
            };
            var cls = Communicator.Current_Schedule.FilterForClass(filters).First();
            if(cls == null)
            {
                return NotFound();
            }
            cls.Students = cls.Students.Where(x => x.ID != body.StudentID).ToList();

        //    Communicator.Current_Schedule.FilterForClass()
            return Ok();
        }

        //public IActionResult OnLaptop()
        [HttpPatch("Instructor")]
        public void UpdateClassInstructor([FromBody] UpdateInstructor body)
        {
            throw new NotImplementedException();
        }

        [HttpGet("History/ByDate")]
        public ActionResult<List<GMRClass>> GetHistoryByDate([FromQuery] TimeRange body)
        {
            throw new NotImplementedException();
        }
        

        [HttpGet("History/ByID")]
        public ActionResult<List<GMRClass>> GetStudentHistory([FromQuery] StudentInfo body)
        {
            throw new NotImplementedException();
        }

        [HttpGet("History/ByStudentRecord")]
        public ActionResult<List<GMRClass>> GetStudentHistoryByDate([FromQuery] StudentRecord body)
        {
            throw new NotImplementedException();
        }
    }
}
