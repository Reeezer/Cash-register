using System;
using SQLite;

namespace CashRegister.Model
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public int Role { get; set; } // 0: customer, 1: seller, 2: admin

        public User() { }
    }
}
