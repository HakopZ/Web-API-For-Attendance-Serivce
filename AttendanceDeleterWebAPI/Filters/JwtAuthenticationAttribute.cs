using AttendanceWebAPI;
using Microsoft.AspNetCore.Server.HttpSys;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;

namespace Test_2.Filters
{
    class HttpActionError : IHttpActionResult
    {
        public string ErrorMessage { get; set; }
        public HttpRequestMessage Request { get; set; }
        public HttpActionError(string errorMessage, HttpRequestMessage req)
        {
            ErrorMessage = errorMessage; 
            Request = req;
        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
    public class JwtAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple => true;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            if (authorization != null || authorization?.Scheme != "Bearer")
            {
                return;
            }

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new HttpActionError("No JWT Token Found", request);
                return;
            }

            var token = authorization.Parameter;
            var principal = await AuthenticateToken(token);

            if (principal == null)
            {
                context.ErrorResult = new HttpActionError("Invalid token", request);
            }
            else
            {
                context.Principal = principal;
            }
        }
        private static bool ValidateToken(string token, out string username)
        {
            username = "";

            var simplePrinciple = JwtManager.GetPrincipal(token);
            var identity = simplePrinciple?.Identity as ClaimsIdentity;

            if (identity == null)
                return false;

            if (!identity.IsAuthenticated)
                return false;

            var usernameClaim = identity.FindFirst(ClaimTypes.Name);
            if (usernameClaim == null)
            {
                username = "";
            }
            else
            {
                username = usernameClaim.Value;
            }
            if (string.IsNullOrEmpty(username))
                return false;

            // More validate to check whether username exists in system

            return true;
        }

        private Task<IPrincipal> AuthenticateToken(string token)
        {
            if (ValidateToken(token, out var username))
            {
                // based on username to get more information from database in order to build local identity
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username)
                    // Add more claims if needed: Roles, ...
                };

                var identity = new ClaimsIdentity(claims, "Jwt");
                IPrincipal user = new ClaimsPrincipal(identity);

                return Task.FromResult(user);
            }

            return Task.FromResult<IPrincipal>(null);
        }
        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
      
    }
}
