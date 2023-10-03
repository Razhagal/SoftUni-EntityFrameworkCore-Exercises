using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Transactions;

/*
 * Write a program that reads information about a minion and its villain and adds it to the database.
 * In case the town of the minion is not in the database, insert it as well.
 * In case the villain is not present in the database, add him too with a default evilness factor of "evil".
 * Finally set the new minion to be a servant of the villain. Print appropriate messages after each operation.
 * 
 * •	On the first line, you will receive the minion information in the format "Minion: <Name> <Age> <TownName>"
 * •	On the second – the villain information in the format "Villain: <Name>"
 */

namespace AddMinion
{
    internal class AddMinion
    {
        private const string DefaultVillainEvilness = "evil";

        static void Main(string[] args)
        {
            string[] minionInput = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string[] villainInput = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);

            string newMinionName = minionInput[1];
            int newMinionAge = int.Parse(minionInput[2]);
            string newMinionTownName = minionInput[3];

            string villainName = villainInput[1];

            SqlConnection connection = new SqlConnection(@"Server=.; Database=MinionsDB; Integrated Security=true; TrustServerCertificate=true");
            connection.Open();

            using (connection)
            {
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    object townId = FetchTownId(newMinionTownName, connection, transaction);
                    if (townId == null)
                    {
                        AddNewTown(newMinionTownName, connection, transaction);
                        townId = FetchTownId(newMinionTownName, connection, transaction);
                    }

                    object villainId = FetchVillainId(villainName, connection, transaction);
                    if (villainId == null)
                    {
                        AddNewVillain(villainName, connection, transaction);
                        villainId = FetchVillainId(villainName, connection, transaction);
                    }

                    object minionId = FetchMinionId(newMinionName, connection, transaction);
                    if (minionId == null)
                    {
                        AddNewMinion(newMinionName, newMinionAge, (int)townId, connection, transaction);
                        minionId = FetchMinionId(newMinionName, connection, transaction);

                        MapMinionVillain((int)minionId, (int)villainId, connection, transaction);
                        Console.WriteLine($"Successfully added {newMinionName} to be minion of {villainName}.");
                    }

                    transaction.Commit();
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

        private static object FetchTownId(string townName, SqlConnection currentConnection, SqlTransaction ongoingTransaction)
        {
            string query = @"SELECT Id FROM Towns WHERE [Name] = @TownName";
            SqlCommand command = new SqlCommand(query, currentConnection, ongoingTransaction);
            command.Parameters.AddWithValue("@TownName", townName);

            var townId = command.ExecuteScalar();

            command.Dispose();
            return townId;
        }

        private static object FetchVillainId(string villainName, SqlConnection currentConnection, SqlTransaction ongoingTransaction)
        {
            string query = @"SELECT Id FROM Villains WHERE [Name] = @VillainName";
            SqlCommand command = new SqlCommand(query, currentConnection, ongoingTransaction);
            command.Parameters.AddWithValue("@VillainName", villainName);

            var villainId = command.ExecuteScalar();

            command.Dispose();
            return villainId;

        }

        private static object FetchMinionId(string minionName, SqlConnection currentConnection, SqlTransaction ongoingTransaction)
        {
            string query = @"SELECT Id FROM Minions WHERE [Name] = @MinionName";
            SqlCommand command = new SqlCommand(query, currentConnection, ongoingTransaction);
            command.Parameters.AddWithValue("@MinionName", minionName);

            var minionId = command.ExecuteScalar();

            command.Dispose();
            return minionId;
        }

        private static void AddNewTown(string townName, SqlConnection currentConnection, SqlTransaction ongoingTransaction)
        {
            string query = @"INSERT INTO Towns(Name) VALUES (@TownName)";
            SqlCommand command = new SqlCommand(query, currentConnection, ongoingTransaction);
            command.Parameters.AddWithValue("@TownName", townName);

            using (command)
            {
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    ongoingTransaction.Rollback();
                    Console.WriteLine(ex.Message);
                    command.Dispose();

                    throw ex;
                }

                Console.WriteLine($"Town {townName} was added to the database.");
            }
        }

        private static void AddNewVillain(string villainName, SqlConnection currentConnection, SqlTransaction ongoingTransaction)
        {
            string query = @"INSERT INTO Villains(Name, EvilnessFactorId) VALUES (@VillainName, (SELECT Id FROM EvilnessFactors ef WHERE ef.Name = @EvilnessName))";
            SqlCommand command = new SqlCommand(query, currentConnection, ongoingTransaction);
            command.Parameters.AddWithValue("@VillainName", villainName);
            command.Parameters.AddWithValue("EvilnessName", DefaultVillainEvilness);

            using (command)
            {
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    ongoingTransaction.Rollback();
                    Console.WriteLine(ex.Message);
                    throw ex;
                }

                Console.WriteLine($"Villain {villainName} was added to the database.");
            }
        }

        private static void AddNewMinion(string minionName, int minionAge, int townId, SqlConnection currentConnection, SqlTransaction ongoingTransaction)
        {
            string query = @"INSERT INTO Minions(Name, Age, TownId) VALUES (@MinionName, @MinionAge, @TownId)";
            SqlCommand command = new SqlCommand(query, currentConnection, ongoingTransaction);
            command.Parameters.AddWithValue("@MinionName", minionName);
            command.Parameters.AddWithValue("@MinionAge", minionAge);
            command.Parameters.AddWithValue("@TownId", townId);

            using (command)
            {
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    ongoingTransaction.Rollback();
                    Console.WriteLine(ex.Message);
                    throw ex;
                }
            }
        }

        private static void MapMinionVillain(int minionId, int villainId, SqlConnection currentConnection, SqlTransaction ongoingTransaction)
        {
            string query = @"INSERT INTO MinionsVillains(MinionId, VillainId) VALUES (@MinionId, @VillainId)";
            SqlCommand command = new SqlCommand(query, currentConnection, ongoingTransaction);
            command.Parameters.AddWithValue("@MinionId", minionId);
            command.Parameters.AddWithValue("VillainId", villainId);

            using (command)
            {
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    ongoingTransaction.Rollback();
                    Console.WriteLine(ex.Message);
                    throw ex;
                }
            }
        }
    }
}