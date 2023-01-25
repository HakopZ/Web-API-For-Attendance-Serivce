using System.ComponentModel.DataAnnotations;

namespace Test_2.Models
{
    public class MonitorInfo
    {
        [Required]
        public string  StationID { get; set; }

        [Required]
        public string AccountName { get; set; }

        [Required]
        public string ForegroundWindowTitle { get; set; }
        
        public string CurrentFileName { get; set; }
        
        [Required]
        public TimeOnly Time { get; set; }
    }
}
