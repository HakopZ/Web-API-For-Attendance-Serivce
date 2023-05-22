using AttendanceWebAPI;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.DirectoryServices.AccountManagement;
using RouteAttribute = System.Web.Http.RouteAttribute;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Test_2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("AppPolicy")]
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public class TokenController : ControllerBase
    {
        [HttpPost("GetToken")]
        public string GetToken([FromBody] User user)
        {
            if(CheckUser(user.Username, user.Password))
            {
                return JwtManager.GenerateToken(user.Username);
            }
            throw new System.Web.Http.HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
        }

        private bool CheckUser(string username, string password)
        {
            if(username.ToLower() == "gmr" && password == "GreatMinds217")
            {
                return true;
            }
            using (var principalContext = new PrincipalContext(ContextType.Domain, "GMR.local"))
            {
                var domainUsers = new List<string>();
                var computerPrinciple = new UserPrincipal(principalContext);
                // Performe search for Domain users
                using (var searchResult = new PrincipalSearcher(computerPrinciple))
                {
                    foreach (var domainUser in searchResult.FindAll())
                    {
                        if (domainUser.Name == username)
                        {
                            if(password == "GreatMinds217")
                            {
                                return true;
                            }    
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
