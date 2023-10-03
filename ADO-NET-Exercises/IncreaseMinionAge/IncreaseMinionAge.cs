using Microsoft.Data.SqlClient;
using System.Text;

/*
 * Read from the console minion IDs separated by space.
 * Increment the age of those minions by 1 and make their names title case.
 * Finally, print the name and the age of all minions in the database, each on a new row in format "<Name> <Age>".
 */

namespace IncreaseMinionAge
{
    internal class IncreaseMinionAge
    {
        static void Main(string[] args)
        {
            int[] minionIds = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            SqlConnection connection = new SqlConnection(@"Server=.; Database=MinionsDB; Integrated Security=true; TrustServerCertificate=true");
            connection.Open();

            using (connection)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < minionIds.Length; i++)
                {
                    sb.Append(minionIds[i]);
                    if (i < minionIds.Length - 1)
                    {
                        sb.Append(", ");
                    }
                }

                string query = $@"UPDATE Minions SET Age += 1, Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)) WHERE Id IN ({sb.ToString()})";
                SqlCommand command = new SqlCommand(query, connection);
                using (command)
                {
                    command.ExecuteNonQuery();
                }

                query = @"SELECT Name, Age FROM Minions";
                command = new SqlCommand(query, connection);
                using (command)
                {
                    SqlDataReader reader = command.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["Name"]} {reader["Age"]}");
                        }
                    }
                }
            }
        }
    }
}