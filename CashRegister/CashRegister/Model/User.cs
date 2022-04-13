using System;
using System.Collections.Generic;
using System.Text;

namespace CashRegister.Model
{
    public class User
    {
        private static int id = 1;

        public int ID { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public int Role { get; set; } // 0: customer, 1: seller, 2: admin

        public User(string firstName, string lastName, DateTime birthDate, string email, int role = 0)
        {
            ID = id++;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Email = email;
            Role = role;
        }
    }
}
