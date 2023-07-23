using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Net;
using System.Web;
using Test_2.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Test_2.Controllers
{
    //[Authorize]
    [Route("[controller]")]
    [ApiController]
    public class SlackBotController : ControllerBase
    {

        private SqlConnection sqlConnection;

        public SlackBotController(SqlConnection connection)
        {
            sqlConnection = connection;
        }
        [HttpGet("CheckConnection")]
        public ActionResult<string> CheckConnection(string challenge)
        {
            if (HttpContext.Connection.RemoteIpAddress is not null)
            {

            }
            return Ok(challenge);

        }


        //Needs to call the SQL most likely (discuss)
        [HttpPatch("UpdateAttendance")]
        public async Task<ActionResult> UpdateStudentAttendance([FromBody] StudentAttendance body)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await Helper.CallStoredProcedure("ChangeStudentAttendance", sqlConnection, new
                SqlParameter("@StudentID", body.StudentID), new SqlParameter("@Status", body.Status), new SqlParameter("@TimeSlotID", body.TimeslotID));
            return Ok();
        }


        //NOT DONE YET BECAUSE WE NEED TO ADD ANOTHER PARAMETER
        //NEED TO MOVE THE STUDENT STILL
        [HttpPatch("Location")]
        public ActionResult UpdateStudentLocation([FromBody] StudentLocation body)
        {
            return Ok();
        }
    }
}
