﻿using System.Collections.Generic;
using CashRegister.Model;
using MySqlConnector;

namespace CashRegister.Database
{
    /// <summary>
    /// Singleton. Repository that contains the receipts on a line basis.
    /// </summary>
    class ReceiptLineRepository
    {
        /// <summary>
        /// Database connection.
        /// </summary>        
        private readonly CashDatabase cashDatabase;
        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static ReceiptLineRepository Instance { get; } = new ReceiptLineRepository();

        /// <summary>
        /// Create a connection to the database and open it.
        /// </summary>
        private ReceiptLineRepository()
        {
            cashDatabase = new CashDatabase();
            cashDatabase.Open();
        }

        ~ReceiptLineRepository()
        {
            cashDatabase.Close();
        }

        /// <summary>
        /// Returns a list of all receiptlines in the database.
        /// </summary>
        /// <returns>List of all receiptlines in the database</returns>
        public List<ReceiptLine> GetAll()
        {
            string querystring = "SELECT * FROM receiptlines";
            MySqlDataReader reader = cashDatabase.ExecuteReader(querystring);

            List<ReceiptLine> receiptLines = new List<ReceiptLine>();
            while (reader.Read())
            {
                Receipt rec = ReceiptRepository.Instance.FindById(reader.GetInt32("receipt"));
                Item item = ItemRepository.Instance.FindById(reader.GetInt32("item"));

                receiptLines.Add(new ReceiptLine
                {
                    Id = reader.GetInt32("id"),
                    Receipt = rec,
                    Item = item,
                    Quantity = reader.GetInt32("quantity")
                });
            }
            reader.Close();

            return receiptLines;
        }

        /// <summary>
        /// Returns a list of all receiptlines matching a receipt.
        /// </summary>
        /// <param name="id">the id of the receipt</param>
        /// <returns>List of all receiptlines in the database</returns>
        public List<ReceiptLine> FindAllByReceipt(int id)
        {
            string querystring = "SELECT * FROM receiptlines WHERE receipt = @receipt";
            MySqlDataReader reader = cashDatabase.ExecuteReader(querystring, new Dictionary<string, object>() {
                { "receipt", id}
            });

            List<ReceiptLine> receiptLines = new List<ReceiptLine>();
            while (reader.Read())
            {
                Receipt rec = ReceiptRepository.Instance.FindById(reader.GetInt32("receipt"));
                Item item = ItemRepository.Instance.FindById(reader.GetInt32("item"));

                receiptLines.Add(new ReceiptLine
                {
                    Id = reader.GetInt32("id"),
                    Receipt = rec,
                    Item = item,
                    Quantity = reader.GetInt32("quantity")
                });
            }
            reader.Close();

            return receiptLines;
        }

        /// <summary>
        /// Inserts a new receiptline into the database. Its Id value is then set to the value of the last inserted id.
        /// </summary>
        /// <param name="receiptline">The receiptline to add</param>
        private void Insert(ReceiptLine receiptLine)
        {
            string querystring = "INSERT INTO receiptlines (receipt, item, quantity) VALUES (@receipt, @item, @quantity)";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "receipt", receiptLine.Receipt.Id },
                { "item", receiptLine.Item.Id },
                { "quantity", receiptLine.Quantity },
            };

            receiptLine.Id = cashDatabase.ExecuteNonQuery(querystring, parameters);
        }

        /// <summary>
        /// Given a receiptline already in the database, update the receiptline in the database with the same Id.
        /// </summary>
        /// <param name="receiptline">The receiptline to update</param>
        private void Update(ReceiptLine receiptLine)
        {
            string querystring = "UPDATE receiptlines SET receipt = @receipt, item = @item, quantity = @quantity WHERE id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "id", receiptLine.Id },
                { "receipt", receiptLine.Receipt.Id },
                { "item", receiptLine.Item.Id },
                { "quantity", receiptLine.Quantity }
            };
            cashDatabase.ExecuteNonQuery(querystring, parameters);
        }

        /// <summary>
        /// Saves the receiptline to the database. If the receiptline does not have an ID it is inserted into the database and its ID attribute is set. If it already has one it is updated.
        /// </summary>
        /// <param name="receiptline">The receiptline to save</param>
        public void Save(ReceiptLine receiptLine)
        {
            if (receiptLine.Id == 0)
            {
                Insert(receiptLine);
            }
            else
            {
                Update(receiptLine);
            }
        }

        /// <summary>
        /// Finds the receiptline with the specified ID.
        /// </summary>
        /// <param name="id">The receiptline's ID</param>
        /// <returns>The receiptline, or null if it was not found</returns>
        public ReceiptLine FindById(int id)
        {
            string querystring = "SELECT * FROM receiptlines WHERE id = @id";
            MySqlDataReader reader = cashDatabase.ExecuteReader(querystring, new Dictionary<string, object>() {
                { "id", id }
            });

            ReceiptLine receiptLine = null;
            if (reader.Read())
            {
                Receipt rec = ReceiptRepository.Instance.FindById(reader.GetInt32("receipt"));
                Item item = ItemRepository.Instance.FindById(reader.GetInt32("item"));

                receiptLine = new ReceiptLine
                {
                    Id = reader.GetInt32("id"),
                    Receipt = rec,
                    Item = item,
                    Quantity = reader.GetInt32("quantity"),
                };
            }
            reader.Close();

            return receiptLine;
        }

        /// <summary>
        /// Deletes the receiptline with the specified ID.
        /// </summary>
        /// <param name="id">The receiptline's ID</param>
        public void Delete(int id)
        {
            string querystring = "DELETE FROM receiptlines WHERE id = @id";
            cashDatabase.ExecuteNonQuery(querystring, new Dictionary<string, object>() {
                { "id", id }
            });
        }
    }
}
