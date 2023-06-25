using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;

using Test_2;
using Test_2.Filters;
using Test_2.Models;
//using FromBodyAttribute = Microsoft.AspNetCore.Mvc.FromBodyAttribute;
//using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
//using HttpPatchAttribute = Microsoft.AspNetCore.Mvc.HttpPatchAttribute;

namespace AttendanceWebAPI.Controllers
{

    //[NTLMAuthentication]
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    [EnableCors("AppPolicy")]
    [Authorize]
    public class ComputerController : ControllerBase
    {
        //Not sure we get classID or if we have to figure it out
        //Dictionary<string, int> stationLoggedInCount = new Dictionary<string, int>();
        //public static ILogger<string> eventLog = new Logger<string>(new LoggerFactory());
        //might be a post call discuss later
        

        [HttpGet("Test")]
        public async Task<IActionResult> Test()
        {

            // var check = Security.IsInGroup(User, "Admin");//--

            //var l = Request.HttpContext.Connection.RemoteIpAddress;

            //IPHostEntry host = Dns.GetHostEntry(l);
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            // Check if the user identity is authenticated
            if (identity != null && identity.IsAuthenticated)
            {
                // Retrieve the user's name
                var userName = identity.Name;

                // You can also retrieve other claims associated with the user
                // For example, to get the user's email:
                var userEmail = identity.FindFirst(ClaimTypes.Email)?.Value;

                // Return the user identity information
             //   return $"User Name: {userName}, Email: {userEmail}";
            }

            return Ok("BOB");
        }

        //LOG IN 
        //MonitorInfo Takes IN MonitorInfo(int stationID, string accountName, string? foregroundWindowTitle, string? currentFileName, DateTime? timeOfRecord)
        [HttpPatch("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] MonitorInfo enterInfo)
        {
            //amount of log ins returned from stored procedured
            var returnVal = new SqlParameter("@DuplicateStationID", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };

            //Calling stored procedure sign in 
            await Helper.CallStoredProcedure("Sign In", new SqlParameter("@StationID", enterInfo.StationID), new SqlParameter("@IsManual", false), new SqlParameter("@Username", enterInfo.AccountName), 
                new SqlParameter("@Date", enterInfo.TimeOfRecord), returnVal);


            //Check if there is a double log in 
            if ((int)returnVal.Value != -1)
            {
                //Event message and notifcations
                Communicator.eventMessages.Enqueue(new EventMessage(enterInfo.StationID, "Double Log In. Someone didn't log off", TimeOnly.FromDateTime(DateTime.Now)));
            }
            Communicator.SessionUpdate = true;
            return Ok();
        }

        //LOG OFF
        [HttpPatch("LogOff")]
        public async Task<IActionResult> LogOff([FromBody] MonitorInfo exitInfo)
        {
            //eventLog.LogInformation("Log Off When There wasn't a log in", (exitInfo.StationID));

            //If there is a late submission of data
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
