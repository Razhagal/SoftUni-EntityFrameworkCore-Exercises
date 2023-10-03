using Microsoft.Data.SqlClient;
using System.Reflection.Metadata;

/*
 * Write a program that prints all minion names from the minions table in the following order:
 * first record, last record, first + 1, last - 1, first + 2, last - 2 … first + n, last - n. 
 */

namespace PrintAllMinionNames
{
    internal class PrintAllMinionNames
    {
        static void Main(string[] args)
        {
            List<string> minionNames = new List<string>();
            SqlConnection connection = new SqlConnection(@"Server=.; Database=MinionsDB; Integrated Security=true; TrustServerCertificate=true");

            connection.Open();
            using (connection)
            {
                string query = @"SELECT Name FROM Minions";
                SqlCommand command = new SqlCommand(query, connection);

                using (command)
                {
                    SqlDataReader reader = command.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            minionNames.Add((string)reader["Name"]);
                        }
                    }
                }
            }

            for (int i = 0; i < minionNames.Count / 2; i++)
            {
                Console.WriteLine(minionNames[i]);
                Console.WriteLine(minionNames[minionNames.Count - 1 - i]);
            }

            if (minionNames.Count % 2 > 0)
            {
                Console.WriteLine(minionNames[minionNames.Count / 2]);
            }
        }
    }
}