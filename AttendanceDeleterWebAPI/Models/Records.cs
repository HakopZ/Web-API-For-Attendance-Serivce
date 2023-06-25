using Microsoft.AspNetCore.Mvc;
using Test_2.ScheduleSetup;

namespace Test_2.Models
{

    public record EventMessage(string StationID, string Message, TimeOnly Time);

    public record StatusInfo(List<Student> StudentsLoggedIn, List<GMRClass> Classes);

    public record StudentInfo(int ID, string FirstName, string LastName);

    public record InstructorInfo(int ID, string Name);

}
