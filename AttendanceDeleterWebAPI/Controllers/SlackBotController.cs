using Microsoft.AspNetCore.Mvc;
using Test_2.FilterClasses;
using Test_2.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Test_2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SlackBotController : ControllerBase
    {
        [HttpGet("SlackBot/CheckConnection")]
        public ActionResult<string> CheckConnection(string challenge)
        {
            return Ok(challenge);
        }

        [HttpPatch("SlackBot/UpdateAttendance")]
        public void UpdateStudentAttendance([FromBody] StudentAttendance body)
        {
            var std = Communicator.Current_Schedule.GetStudent(body.TimeSlotID, body.StudentID);
            
            std.Attended = body.Status;
        }

        [HttpPatch("SlackBot/Location")]
        public ActionResult UpdateStudentLocation([FromBody] StudentLocation body)
        {
            List<IFilter> filters = new List<IFilter>()
            {
                new TimeSlotFilter(body.TimeSlotID),
                new StudentFilter(body.StudentID)
            };
            var cls = Communicator.Current_Schedule.FilterForClass(filters).First();
            if (cls == null)
            {
                return NotFound();
            }
            cls.Students = cls.Students.Where(x => x.ID != body.StudentID).ToList();

            //    Communicator.Current_Schedule.FilterForClass()
            return Ok();
        }
    }
}
