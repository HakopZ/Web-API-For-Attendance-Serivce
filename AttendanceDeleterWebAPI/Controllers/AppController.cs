using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
                if (stds.Count() > 0)
                {
                    students.AddRange(stds);
                }
            }

            List<int> timeSlot = time.ToTimeSlot();

            if (timeSlot.Count > 0)
            {
                var timeSlotsInSchedule = timeSlot.Where(a => Communicator.Current_Schedule.Classes.Any(x => x.TimeSlotID == a));
                List<IFilter> filters = new List<IFilter>();
                List<GMRClass> GMRClasses = new List<GMRClass>();
                foreach (var tSlot in timeSlotsInSchedule)
                {
                    GMRClasses.AddRange(Communicator.Current_Schedule.FilterForClass(new List<IFilter>() { new TimeSlotFilter(tSlot) }));
                }
                if (GMRClasses.Count == 0)
                {
                    return NotFound();
                }
                statusInfo = new StatusInfo(students, GMRClasses);
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
            if (cls == null)
            {
                return NotFound();
            }


            foreach (var student in cls.Students)
            {
                if (student.ID == body.StudentID)
                {
                    cls.Students.Remove(student);
                    var newClass = Communicator.Current_Schedule.FilterForClass(new List<IFilter> { new TimeSlotFilter(body.TimeSlotID), new ClassIDFilter(body.NewClassID) }).First();
                    if (newClass == null)
                    {
                        return NotFound();
                    }
                    var checkForStudent = newClass.Students.Where(x => x.StationID == body.NewStationID);
                    if (checkForStudent.Count() > 0)
                    {
                        var studentToSwap = checkForStudent.First();
                        studentToSwap.StationID = student.StationID; 
                        cls.Students.Add(studentToSwap);
                    }

                    student.StationID = body.NewStationID;
                    newClass.Students.Add(student);
                }
            }
            
            return NotFound();
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
