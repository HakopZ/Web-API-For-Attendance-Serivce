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

        public static async Task<DataTable> CallReader(string procedureName, params SqlParameter[] parameters)
        {
            SqlCommand cmd = new SqlCommand(procedureName, Communicator.sqlConnection);
            DataTable dataTable = new DataTable();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parameters);

            await Communicator.sqlConnection.OpenAsync();
            if (Communicator.sqlConnection.State  != ConnectionState.Open)
            {
                ;
            }

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            try
            {
                adapter.Fill(dataTable);

            }
            catch (SqlException sqlEx)
            {
                throw;
            }
            await Communicator.sqlConnection.CloseAsync();
            adapter.Dispose();

            return dataTable;
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
        public static async Task<List<ScheduledClass>> GetClassesFromReader(DataTable reader)
        {
            List<ScheduledClass> sessions = new List<ScheduledClass>();
            //THIS DOES NOT WORK NEED TO FIGURE OUT INSTRUCTOR IDS
            
            foreach(DataRow rows in reader.Rows)
            {
                int sessionID = (int)rows[0];
                var classReaderTask = CallReader("GetClassInfo", new SqlParameter("@SessionID", sessionID));
                var classReader = await classReaderTask;
                var instructorReader = await CallReader("GetInstructorInfo", new SqlParameter("@SessionID", sessionID));
                List<int> instructors = new List<int>();
                foreach(DataRow row in instructorReader.Rows)
                {
                    instructors.Add((int)row[0]);
                }
                ScheduledClass? temp = null;
                foreach(DataRow classRow in classReader.Rows)
                {
                    var returnReader = await CallReader("IsStudentAttending", new SqlParameter("@StudentID", (int)classRow[0]));

                    temp = new ScheduledClass(sessionID, (int)classRow[0], (int)classRow[1], (int)classRow[2], instructors, (StudentStatus)(int)returnReader.Rows[0][0], (DateTime)classRow[5]);
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
