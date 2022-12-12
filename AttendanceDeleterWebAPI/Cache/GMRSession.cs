namespace Test_2.ScheduleSetup
{
    public class GMRSession
    {
        public List<GMRClass> _classes { get; private set; }
        public int TimeSlotID { get; set; } //might change access level
        public int ID { get; }
        public GMRSession(List<GMRClass> classes, int timeSlotID, int iD)
        {
            _classes = classes;
            TimeSlotID = timeSlotID;
            ID = iD;
        }

        public GMRClass GetClassByID(int classID) => _classes.Where(x => x.ID == classID).First();
    }
}
