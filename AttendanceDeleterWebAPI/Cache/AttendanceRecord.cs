namespace Test_2.ScheduleSetup
{
    //grabbing history from sql
    public record AttendanceRecord(int ID, DateTime Date, int TimeSlotID, int ClassID, int InstructorID, bool Attended);
}
