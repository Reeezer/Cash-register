using System.Collections.Generic;
using CashRegister.Model;
using MySqlConnector;

namespace CashRegister.Database
{
    internal class ItemRepository
    {
        private CashDatabase cashDatabase;

        public ItemRepository()
        {
            cashDatabase = CashDatabase.Instance;
        }

        /// <summary>
        /// Returns a list of all items in the database.
        /// </summary>
        /// <returns>List of all items in the database</returns>
        public List<Item> GetAll()
        {
            string querystring = "SELECT * FROM items";
            MySqlDataReader reader = cashDatabase.ExecuteReader(querystring);

            List<Item> items = new List<Item>();
            while (reader.Read())
            {
                CategoryRepository categoryRepository = new CategoryRepository();
                Category cat = categoryRepository.FindById(reader.GetInt32("category"));

                items.Add(new Item
                {
                    Id = reader.GetInt32("id"),
                    Category = cat, //créer une catégorie à partir d'un INT qui est l'id.
                    Name = reader.GetString("name"),
                    Price = reader.GetDouble("price"),
                    Quantity = reader.GetInt32("quantity"),
                    EAN = reader.GetString("ean")
                });
            }
            reader.Close();

            return items;
        }

        /// <summary>
        /// Returns a list of all items where the search query matches their first name, last name or email.
        /// </summary>
        /// <param name="search">The search query</param>
        /// <returns>A list of the items (might be empty if none are found)</returns>
        public List<Item> FindAll(string search)
        {
            string querystring = "SELECT * FROM items WHERE name LIKE @search";
            MySqlDataReader reader = cashDatabase.ExecuteReader(querystring, new Dictionary<string, object>() {
                { "search", $"%{search}%" }
            });

            List<Item> items = new List<Item>();
            while (reader.Read())
            {
                CategoryRepository categoryRepository = new CategoryRepository();
                Category cat = categoryRepository.FindById(reader.GetInt32("category"));

                items.Add(new Item
                {
                    Id = reader.GetInt32("id"),
                    Category = cat, //créer une catégorie à partir d'un INT qui est l'id.
                    Name = reader.GetString("name"),
                    Price = reader.GetDouble("price"),
                    Quantity = reader.GetInt32("quantity"),
                    EAN = reader.GetString("ean")
                });
            }
            reader.Close();

            return items;
        }

        /// <summary>
        /// Inserts a new item into the database. Its Id value is then set to the value of the last inserted id.
        /// </summary>
        /// <param name="item">The item to add</param>
        private void Insert(Item item)
        {
            string querystring = "INSERT INTO items (category, name, price, quantity, ean) VALUES (@category, @name, @price, @quantity, @ean)";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "category", item.Category.Id },
                { "name", item.Name },
                { "price", item.Price },
                { "quantity", item.Quantity },
                { "ean", item.EAN }
            };

            item.Id = cashDatabase.ExecuteNonQuery(querystring, parameters);
        }

        /// <summary>
        /// Given a item already in the database, update the item in the database with the same Id.
        /// </summary>
        /// <param name="item">The item to update</param>
        private void Update(Item item)
        {
            string querystring = "UPDATE items SET category = @category, name = @name, price = @price, quantity = @quantity, ean = @ean WHERE id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "id", item.Id },
                { "category", item.Category.Id },
                { "name", item.Name },
                { "price", item.Price },
                { "quantity", item.Quantity },
                { "ean", item.EAN }
            };
            cashDatabase.ExecuteNonQuery(querystring, parameters);
        }

        /// <summary>
        /// Saves the item to the database. If the item does not have an ID it is inserted into the database and its ID attribute is set. If it already has one it is updated.
        /// </summary>
        /// <param name="item">The item to save</param>
        public void Save(Item item)
        {
            if (item.Id == 0)
            {
                Insert(item);
            }
            else
            {
                Update(item);
            }
        }

        /// <summary>
        /// Finds the item with the specified ID.
        /// </summary>
        /// <param name="id">The item's ID</param>
        /// <returns>The item, or null if it was not found</returns>
        public Item FindById(int id)
        {
            string querystring = "SELECT * FROM items WHERE id = @id";
            MySqlDataReader reader = cashDatabase.ExecuteReader(querystring, new Dictionary<string, object>() {
                { "id", id }
            });

            Item item = null;
            if (reader.Read())
            {
                CategoryRepository categoryRepository = new CategoryRepository();
                Category cat = categoryRepository.FindById(reader.GetInt32("category"));

                item = new Item
                {
                    Id = reader.GetInt32("id"),
                    Category = cat,
                    Name = reader.GetString("name"),
                    Price = reader.GetDouble("price"),
                    Quantity = reader.GetInt32("quantity"),
                    EAN = reader.GetString("ean")
                };
            }
            reader.Close();

            return item;
        }

        /// <summary>
        /// Deletes the item with the specified ID.
        /// </summary>
        /// <param name="id">The item's ID</param>
        public void Delete(int id)
        {
            string querystring = "DELETE FROM items WHERE id = @id";
            cashDatabase.ExecuteNonQuery(querystring, new Dictionary<string, object>() {
                { "id", id }
            });
        }
    }
}
