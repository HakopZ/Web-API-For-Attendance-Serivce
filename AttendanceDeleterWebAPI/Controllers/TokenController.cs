using AttendanceWebAPI;
using AttendanceWebAPI.Controllers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Web.Http;
using Test_2.Models;
using Test_2.ScheduleSetup;
using RouteAttribute = System.Web.Http.RouteAttribute;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Test_2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("AppPolicy")]
    public class TokenController : ControllerBase
    {
        [Microsoft.AspNetCore.Mvc.HttpGet("GetToken")]
        public string GetToken(string username, string password)
        {
            if(CheckUser(username, password))
            {
                return JwtManager.GenerateToken(username);
            }
            throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
        }

        private bool CheckUser(string username, string password)
        {
            if(username == "GMR" && password == "GreatMinds217")
            {
                return true;
            }
            return false;
        }
    }
}
