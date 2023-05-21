using Microsoft.SqlServer.Server;
using System.Data;
using System.Data.SqlClient;

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


    }
}
