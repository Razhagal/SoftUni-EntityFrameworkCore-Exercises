using Microsoft.Data.SqlClient;
using System.Net;
using System.Text;

/*
 * Write a program that changes all town names to uppercase for a given country. 
 * You will receive one line of input with the name of the country. 
 * Print the number of towns that were changed in the format "<ChangedTownsCount> town names were affected.".
 * On a second line, print the names that were changed, separated with a comma and a space.
 * If no towns were affected (the country does not exist in the database or has no cities connected to it), print "No town names were affected.".
 */

namespace ChangeTownNamesCasing
{
    internal class ChangeTownNamesCasing
    {
        static void Main(string[] args)
        {
            string inputCountryName = Console.ReadLine();

            SqlConnection connection = new SqlConnection(@"Server=.; Database=MinionsDB; Integrated Security=true; TrustServerCertificate=true");
            connection.Open();

            using (connection)
            {
                string query = @"SELECT t.Name
	FROM Towns t
	JOIN Countries c ON t.CountryCode = c.Id
	WHERE c.Name = @CountryName";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CountryName", inputCountryName);

                List<string> towns = new List<string>();
                using (command)
                {
                    SqlDataReader reader = command.ExecuteReader();
                    using (reader)
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string foundTown = (string)reader["Name"];
                                towns.Add(foundTown);
                            }

                            Console.WriteLine($"{towns.Count} town names were affected.");
                            Console.WriteLine($"[{string.Join(", ", towns).ToUpper()}]");
                        }
                        else
                        {
                            Console.WriteLine("No town names were affected.");
                        }
                    }
                }

                if (towns.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < towns.Count; i++)
                    {
                        sb.Append($"'{towns[i]}'");
                        if (i < towns.Count - 1)
                        {
                            sb.Append(", ");
                        }
                    }

                    query = $@"UPDATE Towns SET [Name] = UPPER([Name]) WHERE [Name] IN ({sb.ToString()})";
                    command = new SqlCommand(query, connection);
                    
                    using (command)
                    {
                        //command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}