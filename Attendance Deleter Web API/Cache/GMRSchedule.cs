namespace Test_2.ScheduleSetup
{
    public class GMRSchedule
    {
        public int ID { get; }
        public List<GMRSession> Classes { get; private set; }
        public DateTime Date { get; private set; }
        //ALOT MORE THEN JUST THIS
        public GMRSchedule(List<GMRSession> classes, DateTime date, int id)
        {
            Classes = classes;
            Date = date;
            ID = id;
        }

        public GMRSession GetSessionByTime(int timeSlotID) => Classes.Where(x => x.TimeSlotID == timeSlotID).First();

        public GMRSession GetSessionByID(int id) => Classes.Where(x => x.ID == id).First();
    }
}
