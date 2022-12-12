namespace Test_2.ScheduleSetup
{
    public class Instructor
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ID { get; }

        public Instructor(string firstName, string lastName, int iD)
        {
            FirstName = firstName;
            LastName = lastName;
            ID = iD;
        }
    }
}
