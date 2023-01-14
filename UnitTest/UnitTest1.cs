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
            List<GMRClass> classes = new List<GMRClass>();
            
            List<Instructor> instructors = new List<Instructor>();
            instructors.Add(new Instructor("Hakop", "Zarikyan", 1));
            instructors.Add(new Instructor("Hakop2", "Zarikyan", 2));

            List<Student> students = new List<Student>() { new Student("James", "Smith", "1181", 1, "James.Smith"), new Student("John", "Smith", "1182", 2, "John.Smith") };

            Communicator.timeSlotMap = new Dictionary<int, (DateTime start, DateTime end)>()
            {
                {1, (new DateTime(1, 1, 1, 9, 0, 0), new DateTime(1, 1, 1, 10, 0, 0))},
                {2, (new DateTime(1, 1, 1, 10, 15, 0), new DateTime(1, 1, 1, 11, 30, 0))},
                {3, (new DateTime(1, 1, 1, 11, 45, 0), new DateTime(1, 1, 1, 13, 0, 0))},
            };

            classes.Add(new GMRClass(instructors, students, "118", 1, 1));
            Communicator.Current_Schedule = new GMRSchedule(classes, new DateTime(1, 1, 1, 0, 0, 0), 1);
        }
        [Fact]
        public async void ComputerControllerGet()
        {
            FillSchedule();
            ComputerController computerController = new ComputerController();
            var val = await computerController.LogIn(new MonitorInfo("1181", "John.Smith", "DESKTOP", new DateTime(1, 1, 1, 9, 5, 0)));
            Assert.Equal("Ok", val.ToString());
        }
    }
}