using System.ComponentModel.DataAnnotations;

namespace Test_2.Models
{
    public class UpdateScheduleInfo
    {
        [Required]
        public int StudentID { get; set; }
        
        [Required]
        public int? TimeSlotID { get; set; } 
        
        public int? StationID { get; set; }  
        
        public int? InstructorID { get; set; }
        
        public int? NewTimeSlotID { get; set; }
    }
}
