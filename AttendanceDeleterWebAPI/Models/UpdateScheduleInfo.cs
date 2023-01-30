using System.ComponentModel.DataAnnotations;

namespace Test_2.Models
{
    public class UpdateScheduleInfo
    {
        public int StudentID { get; set; }
        public int StationID { get; set; }  
        public int TimeSlotID { get; set; } 
        public DateOnly Date { get; set; }
        public TimeOnly LogInTime { get; set; }
        public TimeOnly LogOutTime { get; set; }
    }
}
