using Microsoft.Data.SqlClient;

/*
 * Write a program that prints on the console all villains’ names
 * and their number of minions of those who have more than 3 minions ordered descending by number of minions.
 */

namespace VillainNames
{
    internal class VillainNames
    {
        static void Main(string[] args)
        {
            SqlConnection con = new SqlConnection(@"Server=.; Database=MinionsDB; Integrated Security=true; TrustServerCertificate=true");
            con.Open();

            using (con)
            {
                string query = @"SELECT v.Name, COUNT(*) AS [MinionsCount]
            FROM Villains v
            JOIN MinionsVillains mv ON mv.VillainId = v.Id
            GROUP BY v.Name
            HAVING COUNT(*) > 3
            ORDER BY [MinionsCount] DESC";
                SqlCommand command = new SqlCommand(query, con);

                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        string villainName = (string)reader["Name"];
                        int minionsCount = (int)reader["MinionsCount"];
                        Console.WriteLine($"{villainName} - {minionsCount}");
                    }
                }
            }
        }
    }
}