using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CashRegister.Database
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
