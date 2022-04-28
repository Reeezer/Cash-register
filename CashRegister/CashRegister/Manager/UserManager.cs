using CashRegister.Model;
using System;
using System.Collections.Generic;
using System.Text;

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
