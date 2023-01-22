﻿using Test_2.FilterClasses;

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


        public Student GetStudent(int timeSlotID, int studentID)
        {
            List<IFilter> filters = new List<IFilter>
            {
                new TimeSlotFilter(timeSlotID),
                new StudentFilter(studentID),
            };
            var clss = FilterForClass(filters).First();
            return clss.Students.Where(x => x.ID == studentID).First();
        }
          
        public List<GMRClass> FilterForClass(List<IFilter> filters)
        {
            return Classes.Where(x => filters.TrueForAll(f => f.Apply(x))).ToList();
        }

        public (GMRClass gmrClass, Student student) GetClosestClassWithStudent(string username, bool enterCheck)
        {
            
            var classes = Classes.Where(x => x.Students.Any(x => x.Username == username && x.Attended == (!enterCheck)));
            var cls = classes.First();
            cls.GetStudentFromUsername(username, out Student result);
            return (cls, result);
        }

        

    }
}
