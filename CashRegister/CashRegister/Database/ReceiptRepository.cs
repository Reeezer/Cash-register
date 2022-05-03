using System;
using System.Collections.Generic;
using CashRegister.Model;
using MySqlConnector;

namespace CashRegister.Database
{
    internal class ReceiptRepository
    {
        private readonly CashDatabase cashDatabase;
        public static ReceiptRepository Instance { get; } = new ReceiptRepository();

        private ReceiptRepository()
        {
            cashDatabase = new CashDatabase();
            cashDatabase.Open();
        }

        ~ReceiptRepository()
        {
            cashDatabase.Close();
        }

        /// <summary>
        /// Returns a list of all receipts in the database.
        /// </summary>
        /// <returns>List of all receipts in the database</returns>
        public List<Receipt> GetAll()
        {
            string querystring = "SELECT * FROM receipts";
            MySqlDataReader reader = cashDatabase.ExecuteReader(querystring);

            List<Receipt> receipts = new List<Receipt>();
            while (reader.Read())
            {
                User usr = UserRepository.Instance.FindById(reader.GetInt32("client"));
                Discount dis = DiscountRepository.Instance.FindById(reader.GetInt32("discount"));

                receipts.Add(new Receipt
                {
                    Id = reader.GetInt32("id"),
                    Date = reader.GetDateTime("date"),
                    Client = usr,
                    Discount = dis,
                });
            }
            reader.Close();

            return receipts;
        }

        /// <summary>
        /// Inserts a new receipt into the database. Its Id value is then set to the value of the last inserted id.
        /// </summary>
        /// <param name="receipt">The receipt to add</param>
        private void Insert(Receipt receipt)
        {
            string querystring = "INSERT INTO receipts (date, client, discount) VALUES (@date, @client, @discount)";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "date", receipt.Date},
                { "client", receipt.Client.Id},
                { "discount", receipt.Discount.Id },
            };

            receipt.Id = cashDatabase.ExecuteNonQuery(querystring, parameters);
        }

        /// <summary>
        /// Given a receipt already in the database, update the receipt in the database with the same Id.
        /// </summary>
        /// <param name="receipt">The receipt to update</param>
        private void Update(Receipt receipt)
        {
            string querystring = "UPDATE receipts SET date = @date, client = @client, discount = @discount WHERE id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "id", receipt.Id },
                { "client", receipt.Client.Id},
                { "discount", receipt.Discount.Id },
            };
            cashDatabase.ExecuteNonQuery(querystring, parameters);
        }

        /// <summary>
        /// Saves the receipt to the database. If the receipt does not have an ID it is inserted into the database and its ID attribute is set. If it already has one it is updated.
        /// </summary>
        /// <param name="receipt">The receipt to save</param>
        public void Save(Receipt receipt)
        {
            if (receipt.Id == 0)
            {
                Insert(receipt);
            }
            else
            {
                Update(receipt);
            }
        }

        public List<Receipt> FindAllByDate(DateTime date)
        {
            string querystring = "SELECT * FROM receipts WHERE date >= @date_begin AND date < @date_end";
            MySqlDataReader reader = cashDatabase.ExecuteReader(querystring, new Dictionary<string, object>() {
                { "date_begin", date.Date },
                { "date_end", date.Date.AddDays(1) }
            });

            List<Receipt> receipts = new List<Receipt>();
            while (reader.Read())
            {
                User usr = UserRepository.Instance.FindById(reader.GetInt32("client"));
                Discount dis = DiscountRepository.Instance.FindById(reader.GetInt32("discount"));

                receipts.Add(new Receipt
                {
                    Id = reader.GetInt32("id"),
                    Date = reader.GetDateTime("date"),
                    Client = usr,
                    Discount = dis,
                });
            }
            reader.Close();

            return receipts;
        }

        /// <summary>
        /// Finds the receipt with the specified ID.
        /// </summary>
        /// <param name="id">The receipt's ID</param>
        /// <returns>The receipt, or null if it was not found</returns>
        public Receipt FindById(int id)
        {
            string querystring = "SELECT * FROM receipts WHERE id = @id";
            MySqlDataReader reader = cashDatabase.ExecuteReader(querystring, new Dictionary<string, object>() {
                { "id", id }
            });

            Receipt receipt = null;
            if (reader.Read())
            {
                User usr = UserRepository.Instance.FindById(reader.GetInt32("client"));
                Discount dis = DiscountRepository.Instance.FindById(reader.GetInt32("discount"));

                receipt = new Receipt
                {
                    Id = reader.GetInt32("id"),
                    Date = reader.GetDateTime("date"),
                    Client = usr,
                    Discount = dis,
                };
            }
            reader.Close();

            return receipt;
        }

        /// <summary>
        /// Deletes the receipt with the specified ID.
        /// </summary>
        /// <param name="id">The receipt's ID</param>
        public void Delete(int id)
        {
            string querystring = "DELETE FROM receipts WHERE id = @id";
            cashDatabase.ExecuteNonQuery(querystring, new Dictionary<string, object>() {
                { "id", id }
            });
        }
    }
}
