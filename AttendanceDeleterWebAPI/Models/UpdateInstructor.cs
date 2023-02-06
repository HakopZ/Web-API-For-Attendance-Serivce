using System.ComponentModel.DataAnnotations;

namespace Test_2.Models
{
    public class UpdateInstructor
    {
        [Required]
        public int OldTeachingStationID { get; set; }

        [Required]
        public int NewTeachingStationID { get; set; }

        [Required]
        public int InstructorID { get; set; }

        
        public int ReplacementID { get; set; }
    }
}
