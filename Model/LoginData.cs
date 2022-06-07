using System;
using System.Collections.Generic;
using System.Text;

namespace OneMorePageObject
{
    public class LoginData
    {
        public LoginData(string login, string password)
        {
            Login = login;
            Password = password;
        }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
