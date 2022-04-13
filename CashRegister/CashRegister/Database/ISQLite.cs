using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CashRegister.Database
{
    internal interface ISQLite
    {        
        SQLiteConnection GetConnection();
    }
}
