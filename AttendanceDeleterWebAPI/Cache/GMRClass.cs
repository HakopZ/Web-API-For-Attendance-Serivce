namespace Test_2.ScheduleSetup
{
    public class GMRClass
    {
        public string Name { get; set; } //access level might change
        public int ID { get; }

        public List<Student> Students { get; set; }
        public List<Instructor> Instructors { get; set; }
        //might be pointless constructor
        public GMRClass(List<Instructor> instructors, string name, int classID)
        {
            Students = new List<Student>();
            Instructors = instructors;
            ID = classID;
            Name = name;

        }
        public GMRClass(List<Instructor> instructors, List<Student> students, string name, int classID)
        {
            Students = students;
            Instructors = instructors;
            Name = name;
            ID = classID;
        }


    }
}
