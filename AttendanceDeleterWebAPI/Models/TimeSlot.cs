using System.ComponentModel.DataAnnotations;

namespace Test_2.Models
{
    public class Timeslot
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        public Timeslot(int id, DateTime start, DateTime end)
        {
            ID = id;
            Start = start;
            End = end;
        }
    }
}
