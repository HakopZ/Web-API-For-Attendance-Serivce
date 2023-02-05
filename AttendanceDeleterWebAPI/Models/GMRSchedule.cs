using System.ComponentModel.DataAnnotations;
using Test_2.FilterClasses;
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
        
    }
}
