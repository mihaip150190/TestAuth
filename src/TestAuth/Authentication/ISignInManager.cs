using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAuth.Authentication
{
    public interface ISignInManager<TUser> where TUser : BasicUser
    {
        Task<SignInResult> PasswordSignInAsync(string email, string password, bool rememberMe);
        Task SignInAsync(TUser user, bool isPersistent, string authenticationMethod = null);
        Task SignOutAsync();
    }
}
