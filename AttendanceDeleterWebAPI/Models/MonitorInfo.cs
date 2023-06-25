using System.ComponentModel.DataAnnotations;

namespace Test_2.Models
{
    public class MonitorInfo
    {
        [Required]
        public string StationName { get; set; }

        [Required]
        public string AccountName { get; set; }

        

        public DateTime? TimeOfRecord { get; set; }
        public MonitorInfo(string stationName, string accountName, string? foregroundWindowTitle, string? currentFileName, DateTime? timeOfRecord)
        {
            StationName = stationName;
            AccountName = accountName;
            TimeOfRecord = timeOfRecord;
        }
    }
}
