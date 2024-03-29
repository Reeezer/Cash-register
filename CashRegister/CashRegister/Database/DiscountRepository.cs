﻿using System.Collections.Generic;
using CashRegister.Model;
using MySqlConnector;

namespace CashRegister.Database
{
    /// <summary>
    /// Singleton. Repository for the discounts.
    /// </summary>
    internal class DiscountRepository
    {
        /// <summary>
        /// Database connection.
        /// </summary>
        private readonly CashDatabase cashDatabase;
        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static DiscountRepository Instance { get; } = new DiscountRepository();

        /// <summary>
        /// Create a connection to the database and open it.
        /// </summary>
        private DiscountRepository()
        {
            cashDatabase = new CashDatabase();
            cashDatabase.Open();
        }

        ~DiscountRepository()
        {
            cashDatabase.Close();
        }

        /// <summary>
        /// Returns a list of all discounts in the database.
        /// </summary>
        /// <returns>List of all discounts in the database</returns>
        public List<Discount> GetAll()
        {
            string querystring = "SELECT * FROM discounts";
            MySqlDataReader reader = cashDatabase.ExecuteReader(querystring);

            List<Discount> discounts = new List<Discount>();
            while (reader.Read())
            {
                Category cat = CategoryRepository.Instance.FindById(reader.GetInt32("category"));

                discounts.Add(new Discount
                {
                    Id = reader.GetInt32("id"),
                    StartDate = reader.GetDateTime("startdate"),
                    EndDate = reader.GetDateTime("enddate"),
                    Category = cat,
                    Percentage = reader.GetInt32("percentage"),
                });
            }
            reader.Close();

            return discounts;
        }

        /// <summary>
        /// Inserts a new discount into the database. Its Id value is then set to the value of the last inserted id.
        /// </summary>
        /// <param name="discount">The discount to add</param>
        private void Insert(Discount discount)
        {
            string querystring = "INSERT INTO discounts (startdate, enddate, category, percentage) VALUES (@startdate, @enddate, @category, @percentage)";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "startdate", discount.StartDate },
                { "enddate", discount.EndDate },
                { "category", discount.Category.Id },
                { "percentage", discount.Percentage }
            };

            discount.Id = cashDatabase.ExecuteNonQuery(querystring, parameters);
        }

        /// <summary>
        /// Given a discount already in the database, update the discount in the database with the same Id.
        /// </summary>
        /// <param name="discount">The discount to update</param>
        private void Update(Discount discount)
        {
            string querystring = "UPDATE discounts SET startdate = @startdate, enddate = @enddate, category = @category, percentage = @percentage WHERE id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "id", discount.Id },
                { "startdate", discount.StartDate },
                { "enddate", discount.EndDate },
                { "category", discount.Category.Id },
                { "percentage", discount.Percentage }
            };
            cashDatabase.ExecuteNonQuery(querystring, parameters);
        }

        /// <summary>
        /// Saves the discount to the database. If the discount does not have an ID it is inserted into the database and its ID attribute is set. If it already has one it is updated.
        /// </summary>
        /// <param name="discount">The discount to save</param>
        public void Save(Discount discount)
        {
            if (discount.Id == 0)
            {
                Insert(discount);
            }
            else
            {
                Update(discount);
            }
        }

        /// <summary>
        /// Finds the discount with the specified ID.
        /// </summary>
        /// <param name="id">The discount's ID</param>
        /// <returns>The discount, or null if it was not found</returns>
        public Discount FindById(int id)
        {
            string querystring = "SELECT * FROM discounts WHERE id = @id";
            MySqlDataReader reader = cashDatabase.ExecuteReader(querystring, new Dictionary<string, object>() {
                { "id", id }
            });

            Discount discount = null;
            if (reader.Read())
            {
                Category cat = CategoryRepository.Instance.FindById(reader.GetInt32("category"));

                discount = new Discount
                {
                    Id = reader.GetInt32("id"),
                    StartDate = reader.GetDateTime("startdate"),
                    EndDate = reader.GetDateTime("enddate"),
                    Category = cat,
                    Percentage = reader.GetInt32("percentage"),
                };
            }
            reader.Close();

            return discount;
        }

        /// <summary>
        /// Deletes the discount with the specified ID.
        /// </summary>
        /// <param name="id">The discount's ID</param>
        public void Delete(int id)
        {
            string querystring = "DELETE FROM discounts WHERE id = @id";
            cashDatabase.ExecuteNonQuery(querystring, new Dictionary<string, object>() {
                { "id", id }
            });
        }
    }
}
