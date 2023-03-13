using System.ComponentModel.DataAnnotations;

namespace Test_2.ScheduleSetup
{
    public class Student
    {
        [Required]
        public string FirstName { get; private set; }

        [Required]
        public string LastName { get; private set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public int ID { get; }
        
        public Student(int id, string firstName, string lastName, string username)
        {
            FirstName = firstName;
            LastName = lastName;
            ID = id;
            Username = username;
        }

        public Student(int id, string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            ID = id;
            Username = firstName + "." + lastName;
        }
    }
}
