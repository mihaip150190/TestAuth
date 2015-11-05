using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAuth.Authentication
{
    public interface IUserManager
    {
         Task<IdentityResult> CreateAsync(BasicUser user, string password);
         Task<string> GetRoleByIdAsync(int roleId);
         Task<BasicUser> FindByNameAsync(string userName);
         bool CheckPassword(BasicUser user, string password);
    }
}
