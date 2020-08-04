using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ISMNewsPortal.Models
{
    public class UserManager : UserManager<User, int>
    {
        public UserManager(IUserStore<User, int> store)
        : base(store)
        {
            UserValidator = new UserValidator<User, int>(this);
            PasswordValidator = new PasswordValidator() { };
        }

        protected override async Task<bool> VerifyPasswordAsync(IUserPasswordStore<User, int> store, User user,
            string password)
        {
            var hash = await store.GetPasswordHashAsync(user);
            return hash == password;
        }

        public static IUserStore<User, int> GetUsers
        {
            get { return new IdentityStore(); }
        }
    }
}