using Microsoft.AspNetCore.Mvc;

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
}
