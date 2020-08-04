using ISMNewsPortal.Helpers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ISMNewsPortal.Models
{
    public class IdentityStore : IUserStore<User, int>, IUserPasswordStore<User, int>,
    IUserLockoutStore<User, int>, IUserTwoFactorStore<User, int>
    {
        public Task CreateAsync(User user)
        {
            return Task.Run(() => AdminHelper.CreateAdmin(user));
        }

        public Task DeleteAsync(User user)
        {
            return Task.Run(() => AdminHelper.DeleteAdmin(user.Id));
        }

        public void Dispose()
        {
            // do something
        }

        public Task<User> FindByIdAsync(int userId)
        {
            return Task.Run(() =>
            {
                try
                {
                    return new User(AdminHelper.GetAdmin(userId));
                }
                catch
                {
                    return null;
                }
            });
        }

        public Task<User> FindByNameAsync(string userName)
        {
            return Task.Run(() =>
            {
                var admin = AdminHelper.GetAdmin(userName);
                if (admin != null)
                    return new User(admin);
                return null;
            });
        }

        public Task<int> GetAccessFailedCountAsync(User user)
        {
            return Task.FromResult(0);
        }

        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            return Task.FromResult(false);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            return Task.FromResult(DateTimeOffset.MaxValue);
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            return Task.FromResult(user.Password);
        }

        public Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            return Task.FromResult(false);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult(true);
        }

        public Task<int> IncrementAccessFailedCountAsync(User user)
        {
            return Task.FromResult(0);
        }

        public Task ResetAccessFailedCountAsync(User user)
        {
            return Task.CompletedTask;
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            return Task.CompletedTask;
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            return Task.Run(() => user.Password = passwordHash);
        }

        public Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            return Task.CompletedTask;
        }

        public Task UpdateAsync(User user)
        {
            return Task.Run(() => AdminHelper.UpdateAdmin(user));
        }
    }
}