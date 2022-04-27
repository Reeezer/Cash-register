using CashRegister.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CashRegister.Manager
{
    public class UserManager
    {
        private static UserManager instance = null;
        public User User { get; set; }

        private UserManager()
        {
            
        }

        public static UserManager GetInstance()
        {
            if (instance == null)
            {
                instance = new UserManager();
            }
            return instance;
        }

        public bool IsConnected()
        {
            return User != null;
        }
    }
}
