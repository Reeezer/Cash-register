using System;
using System.Collections.Generic;
using MySqlConnector;

namespace CashRegister.Database
{
    /// <summary>
    /// Singleton that allows connecting to the remote SQL database.
    /// </summary>
    public class CashDatabase
    {
        private const string connectionString = "Server=213.193.78.182;Database=dotnetapp;Uid=root;Pwd=M0nM0tD3P4ss3MySQ12022!;Port=3307;";
        private readonly MySqlConnection sqlConnection;
        
        private CashDatabase()
        {
            sqlConnection = new MySqlConnection(connectionString);
        }
        public static CashDatabase Instance { get; } = new CashDatabase();

        public void Open()
        {
            sqlConnection.Open();
        }

        public void Close()
        {
            sqlConnection.Close();
        }

        public void PrintTables()
        {
            MySqlCommand cmd = new MySqlCommand("SHOW TABLES", sqlConnection);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader.GetString(0));
            }
            reader.Close();
        }

        internal MySqlDataReader ExecuteReader(string command)
        {
            return ExecuteReader(command, new Dictionary<string, object>());
        }

        /// <summary>
        /// Executes a query and returns a reader as a MySqlDataReader to iterate over the data.
        /// </summary>
        /// <param name="command">The query string</param>
        /// <param name="parameters">Dictionary of parameters mapping a key to an object</param>
        /// <returns>A data reader object corresponding to the query</returns>
        internal MySqlDataReader ExecuteReader(string command, Dictionary<string, object> parameters)
        {
            MySqlCommand sqlcommand = new MySqlCommand
            {
                CommandText = command,
                Connection = sqlConnection
            };

            foreach (KeyValuePair<string, object> entry in parameters)
                sqlcommand.Parameters.AddWithValue(entry.Key, entry.Value);

            MySqlDataReader reader = sqlcommand.ExecuteReader();
            return reader;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        /// <returns>ID of last inserted element. Returns 0 if the command did not insert anything.</returns>
        internal int ExecuteNonQuery(string command, Dictionary<string, object> parameters)
        {
            MySqlCommand sqlcommand = new MySqlCommand
            {
                CommandText = command,
                Connection = sqlConnection
            };

            foreach (KeyValuePair<string, object> entry in parameters)
                sqlcommand.Parameters.AddWithValue(entry.Key, entry.Value);

            sqlcommand.ExecuteNonQuery();
            return (int) sqlcommand.LastInsertedId; // convert to int since we use 32 bits IDs in the database
        }
    }   
}
