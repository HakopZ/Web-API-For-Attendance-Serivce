global using Fictionary = System.Collections.Generic.Dictionary<string, int>;

using System.Data.SqlClient;
using Test_2.Models;
using Test_2.ScheduleSetup;

namespace Test_2
{
    public static class Communicator
    {
        public static Uri baseAddress = new Uri("http://gmr-124-2-1:5247/");
        public static string connectionString = "Data Source=GMR-124-2-1;Initial Catalog=Deleter2;Integrated Security=True";
        public static bool SessionUpdate = false;
        public static GMRSchedule? Current_Schedule { get; set; }
        public static List<Timeslot> timeslotMap = new List<Timeslot>();
       
        public static Queue<EventMessage> eventMessages = new Queue<EventMessage>();
       
        public static Fictionary StudentMap = new ();
       // public static bool EventOccured = false;
        
    }
}
