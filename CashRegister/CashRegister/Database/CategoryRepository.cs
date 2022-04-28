using System.Collections.Generic;
using System.Drawing;
using CashRegister.Model;
using MySqlConnector;

namespace CashRegister.Database
{
    internal class CategoryRepository
    {
        private CashDatabase cashDatabase;

        public CategoryRepository()
        {
            cashDatabase = CashDatabase.Instance;
        }

        /// <summary>
        /// Returns a list of all categories in the database.
        /// </summary>
        /// <returns>List of all categories in the database</returns>
        public List<Category> GetAll()
        {
            string querystring = "SELECT * FROM category";
            MySqlDataReader reader = cashDatabase.ExecuteReader(querystring);

            List<Category> category = new List<Category>();
            while (reader.Read())
            {
                category.Add(new Category
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    PrincipalColor = Color.FromName(reader.GetString("principalcolor")),
                    SecondaryColor = Color.FromName(reader.GetString("secondarycolor")),
                    ActualColor = Color.FromName(reader.GetString("actualcolor")),
                });
            }
            reader.Close();

            return category;
        }

        /// <summary>
        /// Returns a list of all categories where the search query matches their first name, last name or email.
        /// </summary>
        /// <param name="search">The search query</param>
        /// <returns>A list of the categories (might be empty if none are found)</returns>
        public List<Category> FindAll(string search)
        {
            string querystring = "SELECT * FROM category WHERE name LIKE @search OR principalcolor LIKE @search OR secondarycolor LIKE @search OR actualcolor LIKE @search";
            MySqlDataReader reader = cashDatabase.ExecuteReader(querystring, new Dictionary<string, object>() {
                { "search", $"%{search}%" }
            });

            List<Category> category = new List<Category>();
            while (reader.Read())
            {
                category.Add(new Category
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    PrincipalColor = Color.FromName(reader.GetString("principalcolor")),
                    SecondaryColor = Color.FromName(reader.GetString("secondarycolor")),
                    ActualColor = Color.FromName(reader.GetString("actualcolor")),
                });
            }
            reader.Close();

            return category;
        }

        /// <summary>
        /// Inserts a new category into the database. Its Id value is then set to the value of the last inserted id.
        /// </summary>
        /// <param name="category">The category to add</param>
        private void Insert(Category category)
        {
            string querystring = "INSERT INTO category (name, principalcolor, secondarycolor, actualcolor) VALUES (@name, @principalcolor, @secondarycolor, @actualcolor)";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "name", category.Name },
                { "principalcolor", category.PrincipalColor.ToString() },
                { "secondarycolor", category.SecondaryColor.ToString() },
                { "actualcolor", category.ActualColor.ToString() }
            };

            category.Id = cashDatabase.ExecuteNonQuery(querystring, parameters);
        }

        /// <summary>
        /// Given a category already in the database, update the category in the database with the same Id.
        /// </summary>
        /// <param name="category">The category to update</param>
        private void Update(Category category)
        {
            string querystring = "UPDATE category SET name = @name, principalcolor = @principalcolor, secondarycolor = @secondarycolor, actualcolor = @actualcolor WHERE id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "id", category.Id },
                { "name", category.Name },
                { "principalcolor", category.PrincipalColor.ToString() },
                { "secondarycolor", category.SecondaryColor.ToString() },
                { "actualcolor", category.ActualColor.ToString() }
            };
            cashDatabase.ExecuteNonQuery(querystring, parameters);
        }

        /// <summary>
        /// Saves the category to the database. If the category does not have an ID it is inserted into the database and its ID attribute is set. If it already has one it is updated.
        /// </summary>
        /// <param name="category">The category to save</param>
        public void Save(Category category)
        {
            if (category.Id == 0)
            {
                Insert(category);
            }
            else
            {
                Update(category);
            }
        }

        /// <summary>
        /// Finds the category with the specified ID.
        /// </summary>
        /// <param name="id">The category's ID</param>
        /// <returns>The category, or null if it was not found</returns>
        public Category FindById(int id)
        {
            string querystring = "SELECT * FROM category WHERE id = @id";
            MySqlDataReader reader = cashDatabase.ExecuteReader(querystring, new Dictionary<string, object>() {
                { "id", id }
            });

            Category category = null;
            if (reader.Read())
            {
                category = new Category
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    PrincipalColor = Color.FromName(reader.GetString("principalcolor")),
                    SecondaryColor = Color.FromName(reader.GetString("secondarycolor")),
                    ActualColor = Color.FromName(reader.GetString("actualcolor")),
                };
            }
            reader.Close();

            return category;
        }

        /// <summary>
        /// Deletes the category with the specified ID.
        /// </summary>
        /// <param name="id">The category's ID</param>
        public void Delete(int id)
        {
            string querystring = "DELETE FROM category WHERE id = @id";
            cashDatabase.ExecuteNonQuery(querystring, new Dictionary<string, object>() {
                { "id", id }
            });
        }
    }
}
