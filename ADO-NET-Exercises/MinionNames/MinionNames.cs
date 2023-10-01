using Microsoft.Data.SqlClient;
using System.Globalization;

/*
 * Write a program that prints on the console all minion names and age for a given villain id, ordered by name alphabetically.
 * If there is no villain with the given ID, print "No villain with ID <VillainId> exists in the database.".
 * If the selected villain has no minions, print "(no minions)" on the second row.
 */

namespace MinionNames
{
    internal class MinionNames
    {
        static void Main(string[] args)
        {
            int inputVillainId = int.Parse(Console.ReadLine());

            SqlConnection con = new SqlConnection(@"Server=.; Database=MinionsDB; Integrated Security=true; TrustServerCertificate=true");
            con.Open();
            using (con)
            {
                string query = @"SELECT [Name]
	FROM Villains
	WHERE Id = @VillainId";
                SqlCommand sqlCommand = new SqlCommand(query, con);
                sqlCommand.Parameters.AddWithValue("@VillainId", inputVillainId);

                object villainName = sqlCommand.ExecuteScalar();
                if (villainName == null)
                {
                    Console.WriteLine($"No villain with ID {inputVillainId} exists in the database.");
                }
                else
                {
                    Console.WriteLine($"Villain: {(string)villainName}");

                    query = @"SELECT m.Name, m.Age
	FROM Minions m
	JOIN MinionsVillains mv ON mv.MinionId = m.Id
	JOIN Villains v ON v.Id = mv.VillainId
	WHERE v.Id = @VillainId
	ORDER BY m.Name";
                    sqlCommand = new SqlCommand(query, con);
                    sqlCommand.Parameters.AddWithValue("@VillainId", inputVillainId);

                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    using (reader)
                    {
                        if (!reader.Read())
                        {
                            Console.WriteLine("(no minions)");
                        }
                        else
                        {
                            int index = 1;
                            string minionName = (string)reader["Name"];
                            int minionAge = (int)reader["Age"];
                            Console.WriteLine($"{index}. {minionName} {minionAge}");

                            while (reader.Read())
                            {
                                index++;
                                minionName = (string)reader["Name"];
                                minionAge = (int)reader["Age"];
                                Console.WriteLine($"{index}. {minionName} {minionAge}");

                            }
                        }
                    }
                }
            }
        }
    }
}