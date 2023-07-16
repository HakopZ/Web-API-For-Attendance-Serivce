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

            //Calling stored procedure sign in 

            var stationVal = await Helper.CallReader("GetStationID", new SqlParameter("@Name", enterInfo.StationName));
            
            int stationId = (int)stationVal.Rows[0][0];
            var rVal = await Helper.CallReader("IsStudentScheduled", new SqlParameter("@StudentID", Communicator.StudentMap[enterInfo.AccountName]));
            
            if ((int)rVal.Rows[0][0] == 0)
            {
                Communicator.eventMessages.Enqueue(new EventMessage(enterInfo.StationName, $"Student: {enterInfo.AccountName} has signed in at {enterInfo.StationName} is not scheduled", TimeOnly.FromDateTime(DateTime.Now)));
            }
            var r = await Helper.CallReader("Sign In", new SqlParameter("@StationID", stationId), new SqlParameter("@IsManual", false), new SqlParameter("@StudentID", Communicator.StudentMap[enterInfo.AccountName]), 
                new SqlParameter("@Date", enterInfo.TimeOfRecord));
            
            //Check if there is a double log in 
            if ((int)r.Rows[0][0] != -1)
            {
                //Event message and notifcations
                Communicator.eventMessages.Enqueue(new EventMessage(enterInfo.StationName, "Double Log In. Someone didn't log off", TimeOnly.FromDateTime(DateTime.Now)));
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
            var stationVal = await Helper.CallReader("GetStationID", new SqlParameter("@Name", exitInfo.StationName));
          
            int stationId = (int)stationVal.Rows[0][0];
            if (exitInfo.TimeOfRecord == null)
            {
                await Helper.CallStoredProcedure("SignOut", new SqlParameter("@StationID", stationId), new SqlParameter("@StudentID", Communicator.StudentMap[exitInfo.AccountName]));
            }
            else
            {
                await Helper.CallStoredProcedure("SignOut", new SqlParameter("@StationID", stationId), new SqlParameter("@Date", exitInfo.TimeOfRecord), new SqlParameter("@StudentID", Communicator.StudentMap[exitInfo.AccountName]));
            }
            Communicator.SessionUpdate = true;
            return Ok();

        }



    }
}
