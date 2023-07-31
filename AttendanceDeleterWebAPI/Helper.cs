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
        public static async Task CallStoredProcedure(string procedureName, SqlConnection connection, params SqlParameter[] parameters)
        {
            await connection.OpenAsync();
            SqlCommand cmd = new SqlCommand(procedureName, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parameters);
            await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();
        }

        public static async Task<DataTable> CallReader(string procedureName, SqlConnection connection, params SqlParameter[] parameters)
        {
            SqlCommand cmd = new SqlCommand(procedureName, connection);
            DataTable dataTable = new DataTable();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parameters);

            await connection.OpenAsync();
            if (connection.State != ConnectionState.Open)
            {
                ;
            }

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);


            adapter.Fill(dataTable);

            await connection.CloseAsync();
            adapter.Dispose();

            return dataTable;
        }

        public static async Task<T> CallGetAPI<T>(HttpClient client, string address)
        {
            HttpResponseMessage httpResponse;
            httpResponse = await client.GetAsync(address);
            httpResponse.EnsureSuccessStatusCode();
            return await httpResponse.Content.ReadAsAsync<T>();
        }


        //might not be needed
        public static List<int> ToTimeslot(this DateTime time)
        {
            List<int> timeslots = new List<int>();
            foreach (var pairs in Communicator.timeslotMap)
            {
                if (pairs.Start.TimeOfDay < time.TimeOfDay && time.TimeOfDay < pairs.End.TimeOfDay)
                {
                    timeslots.Add(pairs.ID);
                }
            }
            return timeslots;
        }
        public static async Task<List<ScheduledClass>> GetClassesFromReader(DataTable reader, SqlConnection connection)
        {
            List<ScheduledClass> sessions = new List<ScheduledClass>();
            //THIS DOES NOT WORK NEED TO FIGURE OUT INSTRUCTOR IDS

            foreach (DataRow rows in reader.Rows)
            {
                int sessionID = (int)rows[0];
                var classReaderTask = CallReader("GetClassInfo", connection, new SqlParameter("@SessionID", sessionID));
                var classReader = await classReaderTask;
                var instructorReader = await CallReader("GetInstructorInfo", connection, new SqlParameter("@SessionID", sessionID));
                List<int> instructors = new List<int>();
                foreach (DataRow row in instructorReader.Rows)
                {
                    instructors.Add((int)row[0]);
                }
                ScheduledClass? temp = null;
                foreach (DataRow classRow in classReader.Rows)
                {
                    var returnReader = await CallReader("IsStudentAttending", connection, new SqlParameter("@StudentID", (int)classRow[0]));

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
