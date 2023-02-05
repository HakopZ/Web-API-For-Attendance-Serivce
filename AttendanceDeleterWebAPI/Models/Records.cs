using Microsoft.AspNetCore.Mvc;
using Test_2.ScheduleSetup;

namespace Test_2.Models
{
    public record UpdateInstructor(int ClassID, int TimeSlotID, int NewInstructorID);

    public record EventMessage(int StationID, string Message, TimeOnly Time);

    public record StatusInfo(List<Student> StudentsLoggedIn, List<GMRClass> Classes);
}
