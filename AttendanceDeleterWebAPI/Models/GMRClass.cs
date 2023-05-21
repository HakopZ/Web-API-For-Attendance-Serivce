using System.ComponentModel.DataAnnotations;
using Test_2.ScheduleSetup;

namespace Test_2.Models
{
    public class GMRClass
    {
        [Required]
        public string Name { get; set; } //access level might change

        [Required] 
        public int ID { get; set; }

        public List<Student> Students { get; set; }

        public List<Instructor> ClassInstructors { get; set; }

        //might be pointless constructor

        public GMRClass(List<Instructor> instructors, string name, int timeslotID, int classID)
        {
            Students = new List<Student>();
            ClassInstructors = instructors;
            ID = classID;
            Name = name;

        }
        public GMRClass(List<Instructor> instructors, List<Student> students, string name, int timeslotID, int classID)
        {
            Students = students;
            ClassInstructors = instructors;
            Name = name;
            ID = classID;
        }


    }
}
