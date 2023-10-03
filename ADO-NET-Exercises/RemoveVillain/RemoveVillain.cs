using Microsoft.Data.SqlClient;

/*
 * Write a program that receives the ID of a villain, deletes him from the database and releases his minions from serving to him.
 * Print on two lines the name of the deleted villain in format "<Name> was deleted." and
 * the number of minions released in format "<MinionCount> minions were released.".
 * Make sure all operations go as planned, otherwise do not make any changes in the database.
 * If there is no villain in the database with the given ID, print "No such villain was found.".
 */

namespace RemoveVillain
{
    internal class RemoveVillain
    {
        static void Main(string[] args)
        {
            int villainIdInput = int.Parse(Console.ReadLine());

            SqlConnection connection = new SqlConnection(@"Server=.;Database=MinionsDB;Integrated Security=true;TrustServerCertificate=true");

            connection.Open();
            using (connection)
            {
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    string query = @"SELECT [Name] FROM Villains WHERE Id = @VillainId";
                    SqlCommand command = new SqlCommand(query, connection, transaction);
                    command.Parameters.AddWithValue("@VillainId", villainIdInput);

                    string? foundVillainName = string.Empty;
                    using (command)
                    {
                        foundVillainName = command.ExecuteScalar() as string;
                    }

                    if (string.IsNullOrEmpty(foundVillainName))
                    {
                        Console.WriteLine("No such villain was found.");
                    }
                    else
                    {
                        query = @"DELETE FROM MinionsVillains WHERE VillainId = @VillainId";
                        command = new SqlCommand(query, connection, transaction);
                        command.Parameters.AddWithValue("@VillainId", villainIdInput);

                        int freedMinionsCount = 0;
                        using (command)
                        {
                            freedMinionsCount = command.ExecuteNonQuery();
                        }

                        query = @"DELETE FROM Villains WHERE Id = @VillainId";
                        command = new SqlCommand(query, connection, transaction);
                        command.Parameters.AddWithValue("@VillainId", villainIdInput);

                        using (command)
                        {
                            command.ExecuteNonQuery();
                            Console.WriteLine($"{foundVillainName} was deleted.");
                        }

                        Console.WriteLine($"{freedMinionsCount} minions were released.");
                    }

                    //transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
                    throw ex;
                }

                transaction.Dispose();
            }
        }
    }
}