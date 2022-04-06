using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;
using System.Text;

namespace CashDatabase
{
    public class CashDatabaseContext : DbContext
    {
        private readonly string connectionString;
        
        public CashDatabaseContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(connectionString);
    }

    public class Program
    {
    
    }
    
}
