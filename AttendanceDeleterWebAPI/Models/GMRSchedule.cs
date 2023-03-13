using System.ComponentModel.DataAnnotations;
using Test_2.Models;

namespace Test_2.ScheduleSetup
{

    public class GMRSchedule
    {
        [Required]
        public int ID { get; }

        [Required]
        public List<GMRSession> Classes { get; set; }

        [Required]
        public DateOnly Date { get; private set; }

        public GMRSchedule(int iD, List<GMRSession> classes, DateOnly date)
        {
            ID = iD;
            Classes = classes;
            Date = date;
        }
    }
}
