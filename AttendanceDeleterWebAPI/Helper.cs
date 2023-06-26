using Microsoft.SqlServer.Server;
using System.Data;
using System.Data.SqlClient;
using Test_2.ScheduleSetup;

namespace Test_2
{
    public static class Helper
    {
        //might need try catch if not connected to sql 
        //store in queue if offline??
        public static async Task CallStoredProcedure(string procedureName, params SqlParameter[] parameters)
        {
            await Communicator.sqlConnection.OpenAsync();
            SqlCommand cmd = new SqlCommand(procedureName, Communicator.sqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parameters);
            await cmd.ExecuteNonQueryAsync();
            await Communicator.sqlConnection.CloseAsync();
        }

        public static async Task<SqlDataReader> CallReader(string procedureName, params SqlParameter[] parameters)
        {
            await Communicator.sqlConnection.OpenAsync();
            SqlCommand cmd = new SqlCommand(procedureName, Communicator.sqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parameters);
            var reader = await cmd.ExecuteReaderAsync();
            await Communicator.sqlConnection.CloseAsync();

            return reader;
        }

        public static async Task<T> CallGetAPI<T>(string address)
        {
            HttpResponseMessage httpResponse;
            httpResponse = await Communicator.client.GetAsync(address);
            httpResponse.EnsureSuccessStatusCode();
            return await httpResponse.Content.ReadAsAsync<T>();
        }
        

        //might not be needed
        public static List<int> ToTimeslot(this DateTime time)
        {
            List<int> timeslots = new List<int>();
            foreach(var pairs in Communicator.timeslotMap)
            {
                if(pairs.Start.TimeOfDay < time.TimeOfDay && time.TimeOfDay < pairs.End.TimeOfDay)
                {
                    timeslots.Add(pairs.ID);
                }
            }
            return timeslots;
        }
        public static async Task<List<ScheduledClass>> GetClassesFromReader(SqlDataReader reader)
        {
            List<ScheduledClass> sessions = new List<ScheduledClass>();
            //THIS DOES NOT WORK NEED TO FIGURE OUT INSTRUCTOR IDS

            while (await reader.ReadAsync())
            {
                int sessionID = (int)reader[0];
                var classReader = await Helper.CallReader("GetClassInfo", new SqlParameter("@SessionID", sessionID));
                var instructorReader = await Helper.CallReader("GetInstructorInfo", new SqlParameter("@SessionID", sessionID));
                List<int> instructors = new List<int>();
                while (await instructorReader.ReadAsync())
                {
                    instructors.Add((int)instructorReader[0]);
                }
                ScheduledClass? temp = null;
                while (await classReader.ReadAsync())
                {
                    var returnReader = await Helper.CallReader("IsStudentAttending", new SqlParameter("@StudentID", (int)classReader[0]));
                    await returnReader.ReadAsync();
                    temp = new ScheduledClass(sessionID, (int)classReader[0], (int)reader[1], (int)reader[2], instructors, (StudentStatus)(int)returnReader[0], (DateTime)reader[5]);
                }
                if (temp == null)
                {
                    throw new InvalidDataException("FAILED to set scheduled class");
                }
                sessions.Add(temp);
            }
            return sessions;
        }

    }
}
