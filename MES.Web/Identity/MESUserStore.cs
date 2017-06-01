using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using MES.Web.Models;

namespace MES.Web.Identity
{
    public class MESUserStore : IUserStore<MESUser>, IUserPasswordStore<MESUser>, IUserClaimStore<MESUser>, IUserLockoutStore<MESUser, String>
    {
        MESDbContext db = new MESDbContext();

        Task IUserClaimStore<MESUser, string>.AddClaimAsync(MESUser user, Claim claim)
        {
            throw new NotImplementedException();
        }

        Task IUserStore<MESUser, string>.CreateAsync(MESUser user)
        {
            throw new NotImplementedException();
        }

        Task IUserStore<MESUser, string>.DeleteAsync(MESUser user)
        {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose()
        {

            db.Dispose();
        }

        Task<MESUser> IUserStore<MESUser, string>.FindByIdAsync(string userId)
        {
            return ((IUserStore<MESUser, string>)this).FindByNameAsync(userId);
        }

        Task<MESUser> IUserStore<MESUser, string>.FindByNameAsync(string userName)
        {
            var u = db.AppUsers.Where(au => au.ID == userName).SingleOrDefault();
            MESUser rs = u == null ? null : new MESUser()
            {
                Id = u.ID,
                Name = u.ID,
                UserName = u.Name,
                Password = u.Passwd
            };

            return Task<MESUser>.FromResult(rs);
        }

        Task<int> IUserLockoutStore<MESUser, string>.GetAccessFailedCountAsync(MESUser user)
        {
            return Task<int>.FromResult(0);
        }

        Task<IList<Claim>> IUserClaimStore<MESUser, string>.GetClaimsAsync(MESUser user)
        {
            return Task<IList<Claim>>.FromResult((IList<Claim>)new List<Claim>());
        }

        Task<bool> IUserLockoutStore<MESUser, string>.GetLockoutEnabledAsync(MESUser user)
        {
            throw new NotImplementedException();
        }

        Task<DateTimeOffset> IUserLockoutStore<MESUser, string>.GetLockoutEndDateAsync(MESUser user)
        {
            throw new NotImplementedException();
        }

        Task<string> IUserPasswordStore<MESUser, string>.GetPasswordHashAsync(MESUser user)
        {
            throw new NotImplementedException();
        }

        Task<bool> IUserPasswordStore<MESUser, string>.HasPasswordAsync(MESUser user)
        {
            throw new NotImplementedException();
        }

        Task<int> IUserLockoutStore<MESUser, string>.IncrementAccessFailedCountAsync(MESUser user)
        {
            throw new NotImplementedException();
        }

        Task IUserClaimStore<MESUser, string>.RemoveClaimAsync(MESUser user, Claim claim)
        {
            throw new NotImplementedException();
        }

        Task IUserLockoutStore<MESUser, string>.ResetAccessFailedCountAsync(MESUser user)
        {
            throw new NotImplementedException();
        }

        Task IUserLockoutStore<MESUser, string>.SetLockoutEnabledAsync(MESUser user, bool enabled)
        {
            throw new NotImplementedException();
        }

        Task IUserLockoutStore<MESUser, string>.SetLockoutEndDateAsync(MESUser user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }

        Task IUserPasswordStore<MESUser, string>.SetPasswordHashAsync(MESUser user, string passwordHash)
        {
            throw new NotImplementedException();
        }

        Task IUserStore<MESUser, string>.UpdateAsync(MESUser user)
        {
            throw new NotImplementedException();
        }
    }
}
