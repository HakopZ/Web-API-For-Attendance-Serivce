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
        public int StationID { get; set; }

        [Required]
        public string InstructorID { get; set; }


        public GMRSession(int studentID, int timeSlotId, int stationID, string instructorID)
        {
            StudentID = studentID;
            TimeSlotID = timeSlotId;
            StationID = stationID;
            InstructorID = instructorID;
        }
    }
}
