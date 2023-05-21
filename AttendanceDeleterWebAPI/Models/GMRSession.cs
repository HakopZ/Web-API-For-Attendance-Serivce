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
        public int TimeSlotID { get; set; }

        [Required]
        public int StationID { get; set; }

        [Required]
        public List<int> InstructorID { get; set; }

        [Required]
        public StudentStatus Status { get; set; }

        public DateTime Date { get; set; }

        public GMRSession(int studentID, int timeSlotId, int stationID, List<int> instructorID, StudentStatus status, DateTime date)
        {
            StudentID = studentID;
            TimeSlotID = timeSlotId;
            StationID = stationID;
            InstructorID = instructorID;
            Status = status;
            Date = date;
        }
    }
}
