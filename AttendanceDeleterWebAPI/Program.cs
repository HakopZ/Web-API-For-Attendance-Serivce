using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AttendanceWebAPI
{
    public class Program
    {

        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme.",
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                       {
                           new OpenApiSecurityScheme
                           {
                               Reference = new OpenApiReference
                               {
                                   Type = ReferenceType.SecurityScheme,
                                   Id = "Bearer"
                               }
                           },
                           new string[] { }
                       }
                });
            });
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


            builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();

            builder.Services.AddAuthentication().AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "gmr",
                    ValidAudience = "MonitorApp",
                    IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(JwtManager.Secret))
                };


            });
            //builder.Services.Addauthorization(options =>
            //{
            //    options.fallbackpolicy = options.defaultpolicy;
            //});
              
            builder.Services.AddAuthorization();
            
            
            var app = builder.Build();

            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {

                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
                });
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