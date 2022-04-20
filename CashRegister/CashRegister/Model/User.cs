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
        public string Password { get; set; }
        public int Role { get; set; } // 0: customer, 1: seller, 2: admin

        public User(string firstName, string lastName, DateTime birthDate, string email, string password, int role)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Email = email;
            Password = password;
            Role = role;
        }

        public User()
        {
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName} (id={Id}; birthdate={BirthDate}; email={Email}; role={Role})";
        }
    }
}
