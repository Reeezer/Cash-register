using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using SQLite;
using System.IO;
using CashRegister.Droid.Database;
using CashRegister.Database;

[assembly:Dependency(typeof(SQLiteDroid))]
namespace CashRegister.Droid.Database
{
    public class SQLiteDroid : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            var dbase = "AppDatabase";
            var dbpath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            var path = Path.Combine(dbpath, dbase);
            var connection = new SQLiteConnection(path);
            return connection;

        }
    }
}