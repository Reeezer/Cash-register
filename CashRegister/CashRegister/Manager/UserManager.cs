using CashRegister.Model;

namespace CashRegister.Manager
{
    public class UserManager
    {
        public static UserManager Instance { get; } = new UserManager();
        public User User { get; set; }

        private UserManager()
        {
        }

        public bool IsConnected()
        {
            return User != null;
        }
    }
}
