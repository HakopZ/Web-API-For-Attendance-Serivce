using System.ComponentModel.DataAnnotations;

namespace Test_2.Models
{
    public class HistoryInfo
    {
        [Required]
        public DateOnly Start { get; set; }
        
        public DateOnly? End { get; set; }

        [Required]
        public int StudentID { get; set; }
    }
}
