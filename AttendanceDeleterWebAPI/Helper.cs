using System.Data.SqlClient;

namespace Test_2
{
    public static class Helper
    {
        public static SqlCommand CallStoredProcedure(string procedureName, params SqlParameter[] parameters)
        {
            SqlCommand cmd = new SqlCommand(procedureName, Communicator.sqlConnection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parameters);
            return cmd;
        }
        public static int ToTimeSlot(this DateTime time)
        {
            foreach(var pairs in Communicator.timeSlotMap)
            {
                if(pairs.Value.start.TimeOfDay < time.TimeOfDay && time.TimeOfDay < pairs.Value.end.TimeOfDay)
                {
                    return pairs.Key;
                }
            }
            return -1;
        }
    }
}
