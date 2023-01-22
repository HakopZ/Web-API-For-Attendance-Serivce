using Test_2.ScheduleSetup;

namespace Test_2.FilterClasses
{
    public class ClassIDFilter : IFilter
    {
        private int ClassID { get; set; }
        public ClassIDFilter(int classID)
        {
            ClassID = classID;
        }

        public bool Apply(GMRClass gmrClass)
        {
            return gmrClass.ID == ClassID;
        }
    }
}
