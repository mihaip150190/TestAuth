using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAuth.Authentication
{
    public interface ISignInManager
    {
        Task<SignInResult> PasswordSignInAsync(string email, string password, bool rememberMe);
        Task SignInAsync(BasicUser user, bool isPersistent, string authenticationMethod = null);
        Task SignOutAsync();
    }
}
