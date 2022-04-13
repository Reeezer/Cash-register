using System;
using SQLite;

namespace CashRegister.Model
{
    public class User
    {
<<<<<<< HEAD
        [PrimaryKey, AutoIncrement]
        public int Id { get; }
        
=======
        private static int id = 1;

        public int ID { get; }
>>>>>>> c94aea6596e882bbc372397dd3b24b34a148f219
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public int Role { get; set; } // 0: customer, 1: seller, 2: admin

        public User() { }
    }
}
