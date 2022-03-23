using System;
using System.Collections.Generic;
using System.Text;

namespace CashRegister.Model
{
    public class User
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BrithDate { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
    }
}
