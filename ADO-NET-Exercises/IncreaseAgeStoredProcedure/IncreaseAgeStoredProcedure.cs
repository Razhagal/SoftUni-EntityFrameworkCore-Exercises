using Microsoft.Data.SqlClient;

/*
 * Create stored procedure usp_GetOlder (directly in the database using Management Studio or any other similar tool)
 * that receives MinionId and increases that minion’s age by 1.
 * Write a program that uses that stored procedure to increase the age of a minion whose id
 * will be given as input from the console. After that print the name and the age of that minion.
 */

namespace IncreaseAgeStoredProcedure
{
    internal class IncreaseAgeStoredProcedure
    {
        static void Main(string[] args)
        {
            int inputMinionId = int.Parse(Console.ReadLine());

            SqlConnection connection = new SqlConnection(@"Server=.; Database=MinionsDB; Integrated Security=true; TrustServerCertificate=true");
            connection.Open();
            using (connection)
            {
                SqlCommand command = new SqlCommand("usp_GetOlder", connection);
                command.Parameters.AddWithValue("@MinionId", inputMinionId);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                using (command)
                {
                    SqlDataReader reader = command.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["Name"]} - {reader["Age"]} years old");
                        }
                    }
                }
            }
        }
    }
}