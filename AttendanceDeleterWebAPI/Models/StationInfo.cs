namespace Test_2.Models
{
    public class StationInfo
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string TeachingStationName { get; set; }
        
        public string Classroom { get; set; }

        public StationInfo(int id, string name, string teachingStationName, string classroom)
        {
            ID = id;
            Name = name;
            Classroom = classroom;
            TeachingStationName = teachingStationName;
        }
    }
}