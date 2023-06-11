using AttendanceWebAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Server.HttpSys;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Test_2.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NTLMAuthentication : Attribute, IAuthorizationFilter
    {

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Check if the request contains the NTLM authorization header


            var authHeader = context.HttpContext.Request.Headers["Authorization"];
            /*if (authHeader != null && authHeader.StartsWith("NTLM"))
            {
                try
                {
                    // Extract the NTLM token from the authorization header
                    byte[] tokenBytes = Convert.FromBase64String(authHeader.Substring(5));
                    // var identity = NtlmAuthenticator.Authenticate(tokenBytes);
                    IIdentity identity = default;
                    // Create the principal and set it on the HttpContext
                    var principal = new GenericPrincipal(identity, null);
                    context.HttpContext.User = principal;
                    return; // Authentication succeeded, allow the request
                }
                catch (Exception ex)
                {
                    // Log and handle any authentication errors
                    Console.WriteLine("NTLM authentication error: " + ex.Message);
                }
            }*/

            // If the authentication fails or no NTLM header is present, deny the request
           
        }
    }
}
