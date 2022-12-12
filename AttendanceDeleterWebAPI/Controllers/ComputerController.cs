using Microsoft.AspNetCore.Mvc;


namespace AttendanceWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComputerController : ControllerBase
    {
        //Not sure we get classID or if we have to figure it out
        [HttpGet("{stationID}, {classID}, {timeEntered}")]
        public void LogIn(int stationID, int classID, DateTime timeEntered)
        {
            throw new NotImplementedException();
        }
        [HttpGet("{stationID}, {classID}, {timeExited}")]
        public void LogOff(int stationID, int classID, DateTime timeExited)
        {
            throw new NotImplementedException();
        }
        [HttpPost("{stationID}, {classID}, {time}, {applicationName}")]
        public void ApplicaitonUpdate(int stationID, int classID, DateTime time, string applicationName)
        {
            throw new NotImplementedException();
        }
    }
}
