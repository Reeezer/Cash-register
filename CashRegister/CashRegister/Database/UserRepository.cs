using System.Collections.Generic;
using CashRegister.Model;
using MySqlConnector;

namespace CashRegister.Database
{
    internal class UserRepository
    {
        private CashDatabase cashDatabase;
        
        public UserRepository()
        {
            cashDatabase = CashDatabase.Instance;
        }

        public List<User> GetAll()
        {
            string querystring = "SELECT * FROM users";
            MySqlDataReader reader = cashDatabase.ExecuteReader(querystring);

            List<User> users = new List<User>();
            while (reader.Read())
            {
                users.Add(new User {
                    Id = reader.GetInt32("id"),
                    FirstName = reader.GetString("firstname"),
                    LastName = reader.GetString("lastname"),
                    Email = reader.GetString("email"),
                    Password = reader.GetString("password"),
                    Role = reader.GetInt32("role")
                });
            }
            
            return users;
        }

        public List<User> FindAll(string search)
        {
            string querystring = "SELECT * FROM users WHERE firstname LIKE @search OR lastname LIKE @search OR email LIKE @search";
            MySqlDataReader reader = cashDatabase.ExecuteReader(querystring, new Dictionary<string, object>() {
                { "search", $"%{search}%" }
            });

            List<User> users = new List<User>();
            while (reader.Read())
            {
                users.Add(new User
                {
                    Id = reader.GetInt32("id"),
                    FirstName = reader.GetString("firstname"),
                    LastName = reader.GetString("lastname"),
                    Email = reader.GetString("email"),
                    Password = reader.GetString("password"),
                    Role = reader.GetInt32("role")
                });
            }

            return users;
        }
    }
}
