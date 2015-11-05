using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Http;
using Microsoft.Framework.OptionsModel;
using Microsoft.AspNet.Http.Authentication;
using System.Security.Claims;

namespace TestAuth.Authentication
{
    public class CustomSignInManager<TUser> : ISignInManager<TUser> where TUser : BasicUser
    {
        private HttpContext _context;
        private IdentityOptions _options;
        private IUserManager<TUser> _userManager;

        public CustomSignInManager(IHttpContextAccessor contextAccessor, IOptions<IdentityOptions> optionsAccessor, IUserManager<TUser> userManager)
        {
            if (contextAccessor == null || contextAccessor.HttpContext == null)
            {
                throw new ArgumentNullException(nameof(contextAccessor));
            }

            _context = contextAccessor.HttpContext;
            _options = optionsAccessor?.Value ?? new IdentityOptions();
            _userManager = userManager;
        }

        public async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool rememberMe)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return SignInResult.Failed;
            }

            return await PasswordSignInAsync(user, password, rememberMe);
        }

        

        public async Task SignInAsync(TUser user, bool isPersistent, string authenticationMethod = null)
        {
            await SignInAsync(user, new AuthenticationProperties { IsPersistent = isPersistent }, authenticationMethod);
        }

        public async Task SignOutAsync()
        {
            await _context.Authentication.SignOutAsync(_options.Cookies.ApplicationCookieAuthenticationScheme);
        }

        private async Task SignInAsync(TUser user, AuthenticationProperties authenticationProperties, string authenticationMethod)
        {
            var userPrincipal = await CreateUserPrincipalAsync(user);
            if (authenticationMethod != null)
            {
                userPrincipal.Identities.First().AddClaim(new Claim(ClaimTypes.AuthenticationMethod, authenticationMethod));
            }
            await _context.Authentication.SignInAsync(_options.Cookies.ApplicationCookieAuthenticationScheme,
                userPrincipal,
                authenticationProperties ?? new AuthenticationProperties());
        }

        private async Task<ClaimsPrincipal> CreateUserPrincipalAsync(BasicUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var id = new ClaimsIdentity(_options.Cookies.ApplicationCookieAuthenticationScheme,
                                        _options.ClaimsIdentity.UserNameClaimType,
                                        _options.ClaimsIdentity.RoleClaimType);
            id.AddClaim(new Claim(_options.ClaimsIdentity.UserIdClaimType, user.UserId.ToString()));
            id.AddClaim(new Claim(_options.ClaimsIdentity.UserNameClaimType, user.Username));
            id.AddClaim(new Claim(_options.ClaimsIdentity.RoleClaimType, await _userManager.GetRoleByIdAsync(user.RoleID)));

            return new ClaimsPrincipal(id);
        }

        private async Task<SignInResult> PasswordSignInAsync(TUser user, string password, bool rememberMe)
        {
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (_userManager.CheckPassword(user, password))
            {
                await SignInAsync(user, rememberMe);
                return SignInResult.Success;
            }
            
            return SignInResult.Failed;
        }
    }
}
