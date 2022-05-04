using CashRegister.Model;

namespace CashRegister.Manager
{
    public class UserManager
    {
        public static UserManager Instance { get; } = new UserManager();
        public User User { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        private UserManager()
        {
        }

        /// <summary>
        /// Check if the user is logged in
        /// </summary>
        /// <returns>is loggeed in</returns>
        public bool IsConnected()
        {
            return User != null;
        }
    }
}
