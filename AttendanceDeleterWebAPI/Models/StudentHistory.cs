using System.ComponentModel.DataAnnotations;

namespace Test_2.Models
{
    public class HistoryInfo
    {
        public DateOnly? Start { get; set; }
        
        public DateOnly? End { get; set; }

        public int? StudentID { get; set; }
    }
}
