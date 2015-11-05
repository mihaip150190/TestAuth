using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAuth.Authentication
{
    public class CustomUser : BasicUser
    {
        public string Email { get; set; }
        public string CustomProperty { get; set; }
    }
}
