using System;
using System.Collections.Generic;
using System.Linq;

namespace SSD.Mobile.Entities
{
    public class Customer
    {
        public Customer()
        {
        }
        public Customer(string account, string password, bool isAdmin)
        {
            Account = account;
            Password = password;
            IsAdmin = isAdmin;
        }
        public string Account { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }
}
