using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Test_2;
using Test_2.Models;
using Test_2.ScheduleSetup;

namespace AttendanceWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComputerController : ControllerBase
    {
        //Not sure we get classID or if we have to figure it out
        //Dictionary<string, int> stationLoggedInCount = new Dictionary<string, int>();
        public static ILogger<string> eventLog = new Logger<string>(new LoggerFactory());
        //might be a post call discuss later
        [HttpPatch("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] MonitorInfo enterInfo)
        {
            var returnVal = new SqlParameter("@DuplicateStationID", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };


            await Helper.CallStoredProcedure("Sign In", new SqlParameter("@StationID", enterInfo.StationID), new SqlParameter("@Username", enterInfo.AccountName), new SqlParameter("@Date", enterInfo.TimeOfRecord), returnVal);

            if ((int)returnVal.Value != -1)
            {
                Communicator.eventMessages.Enqueue(new EventMessage(enterInfo.StationID, "Double Log In. Someone didn't log off", TimeOnly.FromDateTime(DateTime.Now)));
            }
            Communicator.SessionUpdate = true;
            return Ok();
        }


        [HttpPatch("LogOff")]
        public async Task<IActionResult> LogOff([FromBody] MonitorInfo exitInfo)
        {
            //eventLog.LogInformation("Log Off When There wasn't a log in", (exitInfo.StationID));



            if (exitInfo.TimeOfRecord == null)
            {
                await Helper.CallStoredProcedure("SignOut", new SqlParameter("@StationID", exitInfo.StationID));
            }
            else
            {
                await Helper.CallStoredProcedure("SignOut", new SqlParameter("@StationID", exitInfo.StationID), new SqlParameter("@Date", exitInfo.TimeOfRecord), new SqlParameter("@Username", exitInfo.AccountName));
            }
            Communicator.SessionUpdate = true;
            return Ok();

        }



        [HttpPatch("ApplicationUpdate")]
        public async Task<IActionResult> ApplicationUpdate([FromBody] MonitorInfo monitorInfo)
        {
            await Helper.CallStoredProcedure("ApplicationUpdate", new SqlParameter("@StationID", monitorInfo.StationID), new SqlParameter("@Username", monitorInfo.AccountName), new SqlParameter("@ApplicatoonName", monitorInfo.ForegroundWindowTitle));
            Communicator.SessionUpdate = true;
            return Ok();
        }

        [HttpPatch("CurrentFileUpdate")]
        public async Task<IActionResult> CurrentFileUpdate([FromBody] MonitorInfo monitorInfo)
        {
            await Helper.CallStoredProcedure("CurrentFileUpdate", new SqlParameter("@StationID", monitorInfo.StationID), new SqlParameter("@Username", monitorInfo.AccountName), new SqlParameter("@Filename", monitorInfo.CurrentFileName));
            Communicator.SessionUpdate = true;
            return Ok();
        }
    }
}
