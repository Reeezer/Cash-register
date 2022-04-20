using System;
using System.Collections.Generic;
using System.Text;
using MySqlConnector;



namespace CashRegister.Database
{
    public interface IMySQL
    {
        MySqlConnection GetConnection(string myVar);
    }
}
