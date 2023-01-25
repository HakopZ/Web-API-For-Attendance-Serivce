using System.ComponentModel.DataAnnotations;

namespace Test_2.Models
{
    public class StudentLocation
    {
        [Required]
        public int StudentID { get; set; }

        [Required]
        public int TimeSlotID { get; set; }

        [Required]
        public int NewClassID { get; set; }

        [Required]
        public int NewStationID { get; set; }  
    }
}
