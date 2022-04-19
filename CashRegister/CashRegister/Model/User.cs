using System;
using SQLite;

namespace CashRegister.Model
{
    public class User
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public int Role { get; set; } // 0: customer, 1: seller, 2: admin

        public User() 
        { 

        }

        public User(string v1, string v2, DateTime now, string v3)
        {
            FirstName = v1;
            LastName = v2;
            BirthDate = now;
            Email = v3;
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName} (id={Id}; birthdate={BirthDate}; email={Email}; role={Role})";
        }
    }
}
