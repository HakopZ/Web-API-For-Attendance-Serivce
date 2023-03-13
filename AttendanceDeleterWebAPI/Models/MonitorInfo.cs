using System.ComponentModel.DataAnnotations;

namespace Test_2.Models
{
    public class MonitorInfo
    {
        [Required]
        public int  StationID { get; set; }

        [Required]
        public string AccountName { get; set; }

        public string? ForegroundWindowTitle { get; set; }
        
        public string? CurrentFileName { get; set; }
        

        public DateTime? TimeOfRecord { get; set; }
        public MonitorInfo(int stationID, string accountName, string? foregroundWindowTitle, string? currentFileName, DateTime? timeOfRecord)
        {
            StationID = stationID;
            AccountName = accountName;
            ForegroundWindowTitle = foregroundWindowTitle;
            CurrentFileName = currentFileName;
            TimeOfRecord = timeOfRecord;
        }
    }
}
