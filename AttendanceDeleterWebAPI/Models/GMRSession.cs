using System.ComponentModel.DataAnnotations;

namespace Test_2.ScheduleSetup
{
    public class GMRSession
    {
        [Required]
        public int StudentID { get; set; }
        
        [Required]
        public int TimeSlotID { get; set; }

        [Required] 
        public string ClassroomName { get; set; }

        [Required]
        public int StationID { get; set; }

        [Required]
        public List<int> InstructorID { get; set; }

        public DateOnly? Date { get; set; }

        public GMRSession(int studentID, int timeSlotId, string classroomName, int stationID, List<int> instructorID, DateOnly? date = null)
        {
            StudentID = studentID;
            TimeSlotID = timeSlotId;
            StationID = stationID;
            ClassroomName = classroomName;
            InstructorID = instructorID;
            Date = date;
        }
    }
}
