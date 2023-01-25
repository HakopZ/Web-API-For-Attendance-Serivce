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

        [HttpGet("AllClasses")]
        public ActionResult<GMRClass> GetAllClasses()
        {
            throw new NotImplementedException();
        }

        [HttpGet("Session/CheckStatus")]
        public ActionResult<bool> CheckStatus()
        {
            return Ok(Communicator.Current_Schedule.Updated);
        }
        [HttpGet("Session/GetStatus")]
        public ActionResult<StatusInfo> GetStatus([FromBody] DateTime time)
        {
            throw new NotImplementedException();
        }



        [HttpPatch("Student/Location")]
        public ActionResult UpdateStudentLocation([FromBody] StudentLocation body)
        {
            throw new NotImplementedException();
        }

        //public IActionResult OnLaptop()
        [HttpPatch("Instructor")]
        public ActionResult UpdateClassInstructor([FromBody] UpdateInstructor body)
        {
            throw new NotImplementedException();
        }

        [HttpGet("History/ByDate")]
        public ActionResult<List<GMRClass>> GetHistoryByDate([FromBody][Bind("Start", "End")] HistoryInfo info)
        {
            throw new NotImplementedException();
        }


        [HttpGet("History/ByID")]
        public ActionResult<List<GMRClass>> GetStudentHistory([FromBody][Bind("StudentID")] HistoryInfo info)
        {
            throw new NotImplementedException();
        }

        [HttpGet("History/ByStudentRecord")]
        public ActionResult<List<GMRClass>> GetStudentHistoryByDate([FromBody][Bind("Start", "End", "StudentID")] HistoryInfo info)
        {
            throw new NotImplementedException();
        }
    }
}
