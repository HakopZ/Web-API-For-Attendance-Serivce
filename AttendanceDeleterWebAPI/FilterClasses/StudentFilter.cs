using Test_2.ScheduleSetup;

namespace Test_2.FilterClasses
{
    public class StudentFilter : IFilter
    {
        public int StudentID { get; set; }
        public StudentFilter(int studentID)
        {
            StudentID = studentID;
        }
        public bool Apply(GMRClass cls)
        {
            return cls.Students.Any(x => x.ID == StudentID);
        }
    }
}
