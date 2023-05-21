using System.ComponentModel.DataAnnotations;

namespace Test_2.ScheduleSetup
{
    public enum StudentStatus
    {
        Here,
        NotHere,
        Moved
    }

    public class GMRSession
    {
        [Required]
        public int StudentID { get; set; }
        
        [Required]
        public int TimeslotID { get; set; }

        [Required]
        public int StationID { get; set; }

        [Required]
        public List<int> InstructorIDs { get; set; }

        [Required]
        public StudentStatus Status { get; set; }

        public DateTime Date { get; set; }

        public GMRSession(int studentID, int timeslotId, int stationID, List<int> instructorIDs, StudentStatus status, DateTime date)
        {
            StudentID = studentID;
            TimeslotID = timeslotId;
            StationID = stationID;
            InstructorIDs = instructorIDs;
            Status = status;
            Date = date;
        }
    }
}
