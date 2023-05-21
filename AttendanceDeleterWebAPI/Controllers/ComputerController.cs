using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http;
using Test_2;
using Test_2.Models;
using Test_2.ScheduleSetup;
using FromBodyAttribute = Microsoft.AspNetCore.Mvc.FromBodyAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPatchAttribute = Microsoft.AspNetCore.Mvc.HttpPatchAttribute;

namespace AttendanceWebAPI.Controllers
{
    public class RestrictDomainAttribute : Attribute, IAuthorizationFilter
    {
        public IEnumerable<string> AllowedHosts { get; }

        public RestrictDomainAttribute(params string[] allowedHosts) => AllowedHosts = allowedHosts;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //  Get host from the request and check if it's in the enumeration of allowed hosts
            string host = context.HttpContext.Request.Host.Host;
            if (!AllowedHosts.Contains(host, StringComparer.OrdinalIgnoreCase))
            {
                
                //  Request came from an authorized host, return bad request
                context.Result = new BadRequestObjectResult("Host is not allowed");
            }
        }
    }
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public class ComputerController : ControllerBase
    {
        //Not sure we get classID or if we have to figure it out
        //Dictionary<string, int> stationLoggedInCount = new Dictionary<string, int>();
        public static ILogger<string> eventLog = new Logger<string>(new LoggerFactory());
        //might be a post call discuss later


        [HttpGet("Test")]
        //[RestrictDomain("localhost", "GMR.local")]
        public async Task<IActionResult> Test()
        {
            
           // var check = Security.IsInGroup(User, "Admin");//--

            //var l = Request.HttpContext.Connection.RemoteIpAddress;
            
            
            //IPHostEntry host = Dns.GetHostEntry(l);
            
            
            using (var principalContext = new PrincipalContext(ContextType.Domain, "GMR.local"))
            {
                var domainUsers = new List<string>();
                var computerPrinciple = new ComputerPrincipal(principalContext);
                // Performe search for Domain users
                using (var searchResult = new PrincipalSearcher(computerPrinciple))
                {
                    foreach (var domainUser in searchResult.FindAll())
                    {
                        if (domainUser.DisplayName != null)
                        {
                            domainUsers.Add(domainUser.DisplayName);
                        }
                    }
                }
            }
            return Ok("BOB");
        }
        [HttpPatch("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] MonitorInfo enterInfo)
        {
            var returnVal = new SqlParameter("@DuplicateStationID", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };


            await Helper.CallStoredProcedure("Sign In", new SqlParameter("@StationID", enterInfo.StationID), new SqlParameter("@Username", enterInfo.AccountName), 
                new SqlParameter("@Date", enterInfo.TimeOfRecord), returnVal);

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
