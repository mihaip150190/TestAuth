using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using TestAuth.AuthModels;
using AutoMapper;
using System.Linq;
using Microsoft.Data.Entity;
using AutoMapper.QueryableExtensions;

namespace TestAuth.Authentication
{
    public class CustomUserManager : IUserManager
    {
        private AuthContext _context;
        private IPasswordHasher<BasicUser> _hasher;

        public CustomUserManager(AuthContext context, IPasswordHasher<BasicUser> passwordHasher){
            _context = context;
            _hasher = passwordHasher;
        }

        public bool CheckPassword(BasicUser user, string password)
        {
            if (user == null)
            {
                return false;
            }

            var result = _hasher.VerifyHashedPassword(user, user.Password, password);

            var success = result != PasswordVerificationResult.Failed;
            //Log Failed event
            return success;
        }

        public async Task<IdentityResult> CreateAsync(BasicUser user, string password)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException("user");
                }
                if (password == null)
                {
                    throw new ArgumentNullException("password");
                }

                SetPasswordHash(user, password);
                var newUser = Mapper.Map<User>(user);

                _context.User.Add(newUser);
                await _context.SaveChangesAsync();

                return IdentityResult.Success;
            }
            catch(Exception ex)
            {
                return IdentityResult.Failed(new IdentityError()
                {
                    Description = ex.Message
                });
            }
            
        }

        public async Task<BasicUser> FindByNameAsync(string userName)
        {
            return await _context.User.Where(r => r.Username == userName).ProjectTo<BasicUser>().FirstOrDefaultAsync();
        }

        public async Task<string> GetRoleByIdAsync(int roleId)
        {
            return await _context.Role.Where(r => r.RoleID == roleId).Select(s => s.Description).FirstOrDefaultAsync();
        }

        private void SetPasswordHash(BasicUser user, string password)
        {
            user.Password = _hasher.HashPassword(user, password);
        }
    }
}
