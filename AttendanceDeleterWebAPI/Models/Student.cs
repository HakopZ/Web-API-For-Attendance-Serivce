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
        public string StationID { get; set; }

        [Required]
        public int ID { get; }
        
        /*public Student(string firstName, string lastName, string stationID, int id, string username)
        {
            FirstName = firstName;
            LastName = lastName;
            StationID = stationID;
            ID = id;
            CurrentApp = "";
            Username = username;
        }

        public bool LogIn(string station)
        {
            Attended = true;
            return StationID == station;
        }

        public void LogOut()
        {
            Attended = false;
        }*/
    }
}
