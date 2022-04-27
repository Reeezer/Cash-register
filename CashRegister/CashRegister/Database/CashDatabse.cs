using System;
using System.Collections.Generic;
using MySqlConnector;

namespace CashRegister.Database
{
    public class CashDatabase
    {
        private const string connectionString = "Server=213.193.78.182;Database=dotnetapp;Uid=root;Pwd=M0nM0tD3P4ss3MySQ12022!;Port=3307;";
        private MySqlConnection sqlConnection;
        
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
        
        internal MySqlDataReader ExecuteReader(string command, Dictionary<string, object> parameters)
        {
            // FIXME
            Console.WriteLine($"Executing Query {command}");
            
            MySqlCommand sqlcommand = new MySqlCommand();
            sqlcommand.CommandText = command;
            sqlcommand.Connection = sqlConnection;

            foreach (KeyValuePair<string, object> entry in parameters)
                sqlcommand.Parameters.AddWithValue(entry.Key, entry.Value);

            MySqlDataReader reader = sqlcommand.ExecuteReader();
            return reader;
        }
    }   
}
