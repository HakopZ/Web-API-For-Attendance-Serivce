using System.ComponentModel.DataAnnotations;

namespace Test_2.ScheduleSetup
{
    public class GMRSession
    {
        [Required]
        public int StudentID { get; set; }
        
        [Required]
        public int TimeSlotID { get; set; }

        [Required] 
        public string StationName { get; set; }

        [Required]
        public string InstructorName { get; set; }

        [Required]
        public DateOnly Date { get; set; }
    }
}
