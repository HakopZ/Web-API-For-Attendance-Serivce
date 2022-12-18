namespace Test_2.ScheduleSetup
{
    public class Student
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string CurrentApp { get; set; }
        public string Username { get; set; }
        public int StationID { get; set; }
        public int ID { get; }
        public bool Attended { get; set; } = false;
        public DateTime Entered { get; set; }
        public DateTime Exited { get; set; }
        public Student(string firstName, string lastName, int stationID, int id, string username)
        {
            FirstName = firstName;
            LastName = lastName;
            StationID = stationID;
            ID = id;
            CurrentApp = "";
            Username = username;
        }
    }
}
