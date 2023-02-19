using System.Data.SqlClient;
using Test_2.Models;
using Test_2.ScheduleSetup;

namespace Test_2
{
    public static class Communicator
    {
        static string baseAddress = "https://localhost:7247/";
        static string connectionString = "";
        public static bool SessionUpdate = false;
        public static GMRSchedule? Current_Schedule { get; set; }
        public static List<(int timeSlotID, DateTime start, DateTime end)> timeSlotMap = new List<(int, DateTime, DateTime)>();
        public static SqlConnection sqlConnection = new SqlConnection(connectionString);
        public static HttpClient client = new HttpClient();
        public static Queue<EventMessage> eventMessages= new Queue<EventMessage>();
       // public static bool EventOccured = false;
        
    }
}
