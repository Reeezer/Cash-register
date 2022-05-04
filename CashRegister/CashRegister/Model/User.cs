namespace CashRegister.Model
{
    public class User
    {
        /// <summary>
        /// IMPORTANT : Do NOT change the ID manually. It is handled by the DB.
        /// </summary>
        public int Id { get; set; } = 0;

        /// <summary>
        /// The firstname of the User
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// The lastname of the User
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// The Email of the user
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// The password of the User
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The role of the user
        /// </summary>
        public int Role { get; set; } // 0: customer, 1: seller, 2: admin

        /// <summary>
        /// Constructor of the User
        /// </summary>
        /// <param name="firstName">The firstname of the User</param>
        /// <param name="lastName">The lastname of the User</param>
        /// <param name="email">The Email of the user</param>
        /// <param name="password">The password of the User</param>
        /// <param name="role">The role of the user</param>
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

        /// <summary>
        /// Empty constructor
        /// </summary>
        public User()
        {
        }

        /// <summary>
        /// To string method
        /// </summary>
        /// <returns>A string containing all the iformations of the user.</returns>
        public override string ToString()
        {
            return $"{FirstName} {LastName} (id={Id}; email={Email}; password={Password}; role={Role})";
        }
    }
}
