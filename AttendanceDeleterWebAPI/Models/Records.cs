using Microsoft.AspNetCore.Mvc;
using Test_2.ScheduleSetup;

namespace Test_2.Models
{
    public record StudentAttendance(int StudentID, int TimeSlotID, bool Status);

    public record StudentLocation(int StudentID, int TimeSlotID, int NewClassID);

    public record UpdateInstructor(int ClassID, int TimeSlotID, int NewInstructorID);

    public record StudentInfo(int StudentID);

    public record TimeRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; } = DateTime.Now;
    }

    public record StudentRecord(TimeRange Range, StudentInfo StudentInfo);
    
    public record MonitorInfo(string StationID, string AccountName, string ForegroundWindowTitle, string CurrentFileName, DateTime Time);

    public record EventMessage(string StationID, string Message, DateTime Time);

    public record StatusInfo(List<Student> StudentsLoggedIn, List<GMRClass> Classes);
}
