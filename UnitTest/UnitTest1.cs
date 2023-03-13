using AttendanceWebAPI.Controllers;
using System.Collections.Generic;
using Test_2;
using Test_2.ScheduleSetup;
using Xunit;
using System;
using Test_2.Models;

namespace UnitTest
{
    public class UnitTest1
    {
        
        private void FillSchedule()
        {
            List<GMRSession> classes = new List<GMRSession>();
            
            List<Instructor> instructors = new List<Instructor>();
            instructors.Add(new Instructor(1, "Hakop", "Zarikyan"));
            instructors.Add(new Instructor(2, "Hakop2", "Zarikyan"));

            List<Student> students = new List<Student>() { new Student(1, "James", "Smith", "James.Smith"), new Student(2, "John", "Smith", "John.Smith") };

            Communicator.timeSlotMap = new List<TimeSlot>()
            {
                new TimeSlot(1, new DateTime(1, 1, 1, 9, 0, 0), new DateTime(1, 1, 1, 10, 0, 0)),
                new TimeSlot(2, new DateTime(1, 1, 1, 10, 15, 0), new DateTime(1, 1, 1, 11, 30, 0)),
                new TimeSlot(3, new DateTime(1, 1, 1, 11, 45, 0), new DateTime(1, 1, 1, 13, 0, 0)),
            };

            classes.Add(new GMRSession(1, 1, 118, new List<int>{ 1, 2 }, StudentStatus.NotHere));
            Communicator.Current_Schedule = new GMRSchedule(1, classes, new DateOnly(1, 1, 1));
        }
        [Fact]
        public async void ComputerControllerGet()
        {
            FillSchedule();
            ComputerController computerController = new ComputerController();
            var val = await computerController.LogIn(new MonitorInfo(1181, "John.Smith", "DESKTOP", "FILE", new DateTime(1, 1, 1, 9, 5, 0)));
            Assert.Equal("Ok", val.ToString());
        }
    }
}