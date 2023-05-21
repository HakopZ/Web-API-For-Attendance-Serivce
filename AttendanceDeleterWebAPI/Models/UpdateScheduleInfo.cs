using System.ComponentModel.DataAnnotations;

namespace Test_2.Models
{
    public class UpdateScheduleInfo
    {
        [Required]
        public int StudentID { get; set; }
        
        [Required]
        public int? TimeslotID { get; set; } 
        
        public int? StationID { get; set; }  
        
        public int? InstructorID { get; set; }
        
        public int? NewTimeslotID { get; set; }
    }
}
