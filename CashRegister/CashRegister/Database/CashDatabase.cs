using System;
using System.Collections.Generic;
using MySqlConnector;

namespace CashRegister.Database
{
    /// <summary>
    /// Connection to the database. Each instance connects to the same database.
    /// </summary>
    public class CashDatabase
    {
        // FIXME don't hardcode it!
        /// <summary>
        /// Database connection string
        /// </summary
        private const string connectionString = "Server=213.193.78.182;Database=dotnetapp;Uid=root;Pwd=M0nM0tD3P4ss3MySQ12022!;Port=3307;";
        /// <summary>
        /// Connection to the database
        /// </summary>
        private readonly MySqlConnection sqlConnection;
        
        /// <summary>
        /// Set up a new connection, but don't connect to it yet. Use Open() to open the connection.
        /// </summary>
        public CashDatabase()
        {
            sqlConnection = new MySqlConnection(connectionString);
        }

        /// <summary>
        /// Opens a connection the SQL database.
        /// </summary>

        public void Open()
        {
            sqlConnection.Open();
        }

        /// <summary>
        /// Closes the database connection.
        /// </summary>

        public void Close()
        {
            sqlConnection.Close();
        }

        /// <summary>
        /// Prints a list of all tables present in the database to the standard output.
        /// </summary>

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

        /// <summary>
        /// Executes a query and returns a reader as a MySqlDataReader to iterate over the data.
        /// </summary>
        /// <param name="command">The query string</param>
        /// <returns>A data reader object corresponding to the query</returns>
        internal MySqlDataReader ExecuteReader(string command)
        {
            return ExecuteReader(command, new Dictionary<string, object>());
        }

        /// <summary>
        /// Executes a query with paramaters and returns a reader as a MySqlDataReader to iterate over the data.
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
        /// Executes an SQL command with parameters that doesn't return anything.
        /// </summary>
        /// <param name="command">The query string</param>
        /// <param name="parameters">Dictionary of parameters mapping a key to an object</param>
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
            return (int) sqlcommand.LastInsertedId; // cast to int since we use 32 bits IDs in the database
        }
    }   
}
