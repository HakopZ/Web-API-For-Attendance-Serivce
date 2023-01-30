﻿using System.Data;
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
            Communicator.sqlConnection.CreateCommand();
            SqlCommand cmd = new SqlCommand(procedureName, Communicator.sqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parameters);
            await cmd.ExecuteNonQueryAsync();
            await Communicator.sqlConnection.CloseAsync();
        }
        public static List<int> ToTimeSlot(this DateTime time)
        {
            List<int> timeSlots = new List<int>();
            foreach(var pairs in Communicator.timeSlotMap)
            {
                if(pairs.start.TimeOfDay < time.TimeOfDay && time.TimeOfDay < pairs.end.TimeOfDay)
                {
                    timeSlots.Add(pairs.timeSlotID);
                }
            }
            return timeSlots;
        }

        public enum Filter
        {
            TimeSlot,
            Student,
            Instructor,
            Name,
            ID
        }

    }
}
