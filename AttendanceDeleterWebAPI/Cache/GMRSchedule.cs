namespace Test_2.ScheduleSetup
{
    public class GMRSchedule
    {
        public int ID { get; }
        public List<GMRClass> Classes { get; private set; }
        public DateTime Date { get; private set; }
        public bool Updated { get; set; }
        //ALOT MORE THEN JUST THIS
        public GMRSchedule(List<GMRClass> classes, DateTime date, int id)
        {
            Classes = classes;
            Date = date;
            ID = id;
        }

        public List<GMRClass> GetClassesByTime(int timeSlotID) => Classes.Where(x => x.TimeSlotID == timeSlotID).ToList();

        public (GMRClass gmrClass, Student student) GetClosestClassWithStudent(string username, bool entered, DateTime time)
        {
            var classesWithStudent = Classes.Where(x => x.Students.Any(x => x.Username == username));
            IOrderedEnumerable<GMRClass> classes;

            if (entered)
            {
                classes = classesWithStudent.Where(x => Communicator.timeSlotMap[x.TimeSlotID].end.TimeOfDay >= time.TimeOfDay).OrderBy(t => Communicator.timeSlotMap[t.TimeSlotID].end.TimeOfDay);
            }
            else
            {
                classes = classesWithStudent.Where(x => Communicator.timeSlotMap[x.TimeSlotID].start.TimeOfDay <= time.TimeOfDay).OrderBy(t => Communicator.timeSlotMap[t.TimeSlotID].end.TimeOfDay);
            }
            var cls = classes.First();
            var std = cls.Students.Where(x => x.Username == username).First();
            return (cls, std);
        }

        

    }
}
