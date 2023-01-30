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
        //might be pointless constructor

        /*public GMRClass(List<Instructor> instructors, string name, int timeSlotID, int classID)
        {
            Students = new List<Student>();
            Instructors = instructors;
            ID = classID;
            Name = name;

        }
        public GMRClass(List<Instructor> instructors, List<Student> students, string name, int timeSlotID, int classID)
        {
            Students = students;
            Instructors = instructors;
            Name = name;
            ID = classID;
        }*/


    }
}
