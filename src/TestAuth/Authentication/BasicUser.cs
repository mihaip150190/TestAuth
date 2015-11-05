using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAuth.Authentication
{
    public class BasicUser
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleID { get; set; }
        public string Email { get; set; }
    }
}
