using System;
using System.Collections.Generic;

namespace SSD.Framework.Security
{
    public class LoginResult
    {
        public bool Successeded { get; set; }
        public string UGToken { get; set; }
        public string Message { get; set; }
        public string ProfileJson { get; set; }
    }
}
