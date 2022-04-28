using System;

namespace CashRegister.Model
{
    public class User
    {
        /// <summary>
        /// IMPORTANT : Do NOT change the ID manually
        /// </summary>
        public int Id { get; set; } = 0;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Role { get; set; } // 0: customer, 1: seller, 2: admin

        public User(string firstName, string lastName, string email, string password, Role role)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            
            switch(role)
            {
                case Model.Role.Admin:
                    Role = 2;
                    break;
                case Model.Role.Seller:
                    Role = 1;
                    break;
                case Model.Role.Customer:
                    Role = 0;
                    break;
                default:
                    break;
            }
        }

        public User()
        {
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName} (id={Id}; email={Email}; password={Password}; role={Role})";
        }
    }
}
