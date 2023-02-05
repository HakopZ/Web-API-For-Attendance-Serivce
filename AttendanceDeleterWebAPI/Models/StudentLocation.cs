using System.ComponentModel.DataAnnotations;

namespace Test_2.Models
{
    public class StudentLocation
    {
        [Required]
        public int OldSessionID { get; set; }

        [Required]
        public int NewSessionID { get; set; }

        [Required]
        public int InstructorID { get; set; }


        public int? ReplacementID { get; set; }
    }
}
