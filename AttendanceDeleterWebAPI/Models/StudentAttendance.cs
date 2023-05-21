using System.ComponentModel.DataAnnotations;

namespace Test_2.Models
{
    public class StudentAttendance
    {
        [Required]
        public int StudentID { get; set; }

        [Required]
        public int TimeslotID { get; set; }

        [Required]
        public bool Status { get; set; }
    }
}
