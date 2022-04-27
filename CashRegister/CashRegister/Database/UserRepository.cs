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

        /// <summary>
        /// Returns a list of all users in the database.
        /// </summary>
        /// <returns>List of all users in the database</returns>
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
            reader.Close();
            
            return users;
        }

        /// <summary>
        /// Returns a list of all users where the search query matches their first name, last name or email.
        /// </summary>
        /// <param name="search">The search query</param>
        /// <returns>A list of the users (might be empty if none are found)</returns>
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
            reader.Close();
            
            return users;
        }

        /// <summary>
        /// Inserts a new user into the database. Its Id value is then set to the value of the last inserted id.
        /// </summary>
        /// <param name="user">The user to add</param>
        private void Insert(User user)
        {
            string querystring = "INSERT INTO users (firstname, lastname, email, password, role) VALUES (@firstname, @lastname, @email, @password, @role)";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "firstname", user.FirstName },
                { "lastname", user.LastName },
                { "email", user.Email },
                { "password", user.Password },
                { "role", user.Role }
            };

            user.Id = cashDatabase.ExecuteNonQuery(querystring, parameters);
        }

        /// <summary>
        /// Given a user already in the database, update the user in the database with the same Id.
        /// </summary>
        /// <param name="user">The user to update</param>
        private void Update(User user)
        {
            string querystring = "UPDATE users SET firstname = @firstname, lastname = @lastname, email = @email, password = @password, role = @role WHERE id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "id", user.Id },
                { "firstname", user.FirstName },
                { "lastname", user.LastName },
                { "email", user.Email },
                { "password", user.Password },
                { "role", user.Role }
            };
            cashDatabase.ExecuteNonQuery(querystring, parameters);
        }

        /// <summary>
        /// Saves the user to the database. If the user does not have an ID it is inserted into the database and its ID attribute is set. If it already has one it is updated.
        /// </summary>
        /// <param name="user">The user to save</param>
        public void Save(User user)
        {
            if (user.Id == 0)
            {
                Insert(user);
            }
            else
            {
                Update(user);
            }
        }

        /// <summary>
        /// Finds the user with the specified ID.
        /// </summary>
        /// <param name="id">The user's ID</param>
        /// <returns>The user, or null if it was not found</returns>
        public User FindById(int id)
        {
            string querystring = "SELECT * FROM users WHERE id = @id";
            MySqlDataReader reader = cashDatabase.ExecuteReader(querystring, new Dictionary<string, object>() {
                { "id", id }
            });

            User user = null;
            if (reader.Read())
            {
                user = new User
                {
                    Id = reader.GetInt32("id"),
                    FirstName = reader.GetString("firstname"),
                    LastName = reader.GetString("lastname"),
                    Email = reader.GetString("email"),
                    Password = reader.GetString("password"),
                    Role = reader.GetInt32("role")
                };
            }
            reader.Close();
            
            return user;
        }

        /// <summary>
        /// Deletes the user with the specified ID.
        /// </summary>
        /// <param name="id">The user's ID</param>
        public void Delete(int id)
        {
            string querystring = "DELETE FROM users WHERE id = @id";
            cashDatabase.ExecuteNonQuery(querystring, new Dictionary<string, object>() {
                { "id", id }
            });
        }
    }
}
