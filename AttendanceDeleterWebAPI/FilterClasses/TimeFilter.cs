using Test_2.ScheduleSetup;

namespace Test_2.FilterClasses
{
    public class TimeSlotFilter : IFilter
    {
        public int TimeSlot { get; set; }
        public TimeSlotFilter(int timeSlot)
        {
            TimeSlot = timeSlot;
        }
        public bool Apply(GMRClass cls)
        {
            return cls.TimeSlotID == TimeSlot;
        }
    }
}
