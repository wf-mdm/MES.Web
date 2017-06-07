using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace MES.Web.Identity
{
    public class MESUserManager : UserManager<MESUser>
    {
        public MESUserManager() 
            : base(new MESUserStore())
        {
            this.PasswordHasher = new MESPasswordHasher();
        }

        public override bool SupportsUserRole { get { return true; } }

        public override Task<bool> IsLockedOutAsync(string userId)
        {
            return Task<bool>.FromResult(false);
        }

        public override Task<bool> CheckPasswordAsync(MESUser user, string password)
        {
            return Task<bool>.FromResult(user.Password == password);
        }

        public override Task<bool> GetTwoFactorEnabledAsync(string userId)
        {
            return Task<bool>.FromResult(false);
        }

        public override Task<IList<string>> GetRolesAsync(string userId)
        {
            return base.GetRolesAsync(userId);
        }

        public override Task<bool> IsInRoleAsync(string userId, string role)
        {
            return base.IsInRoleAsync(userId, role);
        }
    }
}
