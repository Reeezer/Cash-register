using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

using CashRegister.Model;
using Xamarin.Forms;
using System.Linq;

namespace CashRegister.Database
{
    internal class ItemDB
    {
        private SQLiteConnection sqliteConnection;

        public ItemDB()
        {
            sqliteConnection = DependencyService.Get<ISQLite>().GetConnection();
            sqliteConnection.CreateTable<Item>();
        }

        public IEnumerable<Item> GetItems()
        {
            return (from t in sqliteConnection.Table<Item>() select t).ToList();
        }

        public Item GetItem(int id)
        {
            return sqliteConnection.Table<Item>().FirstOrDefault(t => t.Id == id);
        }

        public int SaveItem(Item item)
        {
            if (item.Id != 0)
            {
                sqliteConnection.Update(item);
                return item.Id;
            }
            else
            {
                return sqliteConnection.Insert(item);
            }
        }

        public int DeleteItem(int id)
        {
            return sqliteConnection.Delete<Item>(id);
        }}
    }
}
