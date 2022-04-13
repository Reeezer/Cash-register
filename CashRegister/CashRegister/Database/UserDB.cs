using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Xamarin.Forms;
using CashRegister.Model;
using System.Linq;

namespace CashRegister.Database
{
    internal class UserDB
    {
        private SQLiteConnection sqliteConnection;

        public UserDB(SQLiteConnection sqliteConnection)
        {
            sqliteConnection = DependencyService.Get<ISQLite>().GetConnection();
            sqliteConnection.CreateTable<User>();
        }

        public IEnumerable<User> GetUsers()
        {
            return (from t in sqliteConnection.Table<User>() select t).ToList();
        }

        public User GetUser(int id)
        {
            return sqliteConnection.Table<User>().FirstOrDefault(t => t.Id == id);
        }

        public int SaveUser(User user)
        {
            if (user.Id != 0)
            {
                sqliteConnection.Update(user);
                return user.Id;
            }
            else
            {
                return sqliteConnection.Insert(user);
            }
        }

        public int DeleteUser(int id)
        {
            return sqliteConnection.Delete<User>(id);
        }
    }
}
