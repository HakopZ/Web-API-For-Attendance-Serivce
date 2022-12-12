using Microsoft.AspNetCore.Mvc;
using Test_2.ScheduleSetup;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Test_2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MagicController : ControllerBase
    {
        public static GMRSchedule? Current_Schedule { get; set; }
        [HttpPost]
        public void MakeSchedule([FromBody] GMRSchedule schedule)
        {
            Current_Schedule = schedule;
        }

        //constantly running task that checks for schedule changes
    }
}
