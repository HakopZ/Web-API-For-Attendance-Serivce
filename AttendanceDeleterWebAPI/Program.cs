using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Sockets;
using System.Web.Http;
using Test_2;
using Test_2.Filters;

namespace AttendanceWebAPI
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AppPolicy",
                    policy =>
                    {
                        policy.AllowAnyOrigin() //this does not work for everyone
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
                
            });
            /*
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity
            });
           */
            //builder.Services.AddAuthentication(I)
            /*builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate(options =>
            {
                options.EnableLdap("GMR.local");
            });*/
            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AppPolicy");
            app.UseResponseCaching();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}