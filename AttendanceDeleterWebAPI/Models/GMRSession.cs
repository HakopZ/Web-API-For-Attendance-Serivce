using System.ComponentModel.DataAnnotations;

namespace Test_2.ScheduleSetup
{
    public enum StudentStatus
    {
        Present = 1,
        Absent = 0,
    }

    public class ScheduledClass
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

        [Required]
        public int ID { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public ScheduledClass(int id, int studentID, int timeslotId, int stationID, List<int> instructorIDs, StudentStatus status, DateTime date)
        {
            ID = id;
            StudentID = studentID;
            TimeslotID = timeslotId;
            StationID = stationID;
            InstructorIDs = instructorIDs;
            Status = status;
            Date = date;
        }
    }
}
