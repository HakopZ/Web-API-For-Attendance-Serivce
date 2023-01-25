using System.ComponentModel.DataAnnotations;

namespace Test_2.Models
{
    public class HistoryInfo
    {
        [Required]
        public TimeOnly Start { get; set; }
        
        public TimeOnly End { get; set; }

        [Required]
        public int StudentID { get; set; }
    }
}
