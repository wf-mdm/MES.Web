using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using MES.Web.Models;
using Intelli.MidW.BizClient;
using Intelli.MidW.Interface;
using System.Data;

namespace MES.Web.Identity
{
    public class MESUserStore : IUserStore<MESUser>, IUserPasswordStore<MESUser>,
        IUserClaimStore<MESUser>, IUserLockoutStore<MESUser, String>,
        IUserRoleStore<MESUser>
    {
        MESDbContext db = new MESDbContext();

        #region IUserStore
        Task IUserStore<MESUser, string>.CreateAsync(MESUser user)
        {
            throw new NotImplementedException();
        }

        Task IUserStore<MESUser, string>.DeleteAsync(MESUser user)
        {
            throw new NotImplementedException();
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
                Name = u.Name,
                UserName = u.ID,
                Password = u.Passwd
            };

            return Task<MESUser>.FromResult(rs);
        }
        #endregion

        #region IUserPasswordStore

        Task<string> IUserPasswordStore<MESUser, string>.GetPasswordHashAsync(MESUser user)
        {
            throw new NotImplementedException();
        }
        Task<bool> IUserPasswordStore<MESUser, string>.HasPasswordAsync(MESUser user)
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

        #endregion

        #region IUserClaimStore
        Task IUserClaimStore<MESUser, string>.AddClaimAsync(MESUser user, Claim claim)
        {
            throw new NotImplementedException();
        }
        Task<IList<Claim>> IUserClaimStore<MESUser, string>.GetClaimsAsync(MESUser user)
        {
            return Task<IList<Claim>>.FromResult((IList<Claim>)new List<Claim>());
        }
        Task IUserClaimStore<MESUser, string>.RemoveClaimAsync(MESUser user, Claim claim)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IUserLockoutStore
        Task<int> IUserLockoutStore<MESUser, string>.GetAccessFailedCountAsync(MESUser user)
        {
            return Task<int>.FromResult(0);
        }

        Task<bool> IUserLockoutStore<MESUser, string>.GetLockoutEnabledAsync(MESUser user)
        {
            throw new NotImplementedException();
        }

        Task<DateTimeOffset> IUserLockoutStore<MESUser, string>.GetLockoutEndDateAsync(MESUser user)
        {
            throw new NotImplementedException();
        }
        Task<int> IUserLockoutStore<MESUser, string>.IncrementAccessFailedCountAsync(MESUser user)
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
        #endregion

        #region IUserRoleStore
        Task IUserRoleStore<MESUser, string>.AddToRoleAsync(MESUser user, string roleName)
        {
            throw new NotImplementedException();
        }
        Task<IList<string>> IUserRoleStore<MESUser, string>.GetRolesAsync(MESUser user)
        {
            BizRequest request = ClientMgr.Instance.CreateRequest("config", "MES", "", "GETUSRAPS", new Dictionary<string, string>()
            {
                { "uid", user.Id},
                { "modid", "MESADMIN"}
            });
            request.UserId = user.Id;


            return Task<List<String>>.Run(() =>
            {
                IList<String> roles = new List<string>();
                try
                {
                    DataSet ds = ClientMgr.Instance.RunDbCmd(request.CmdName, request);
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        roles.Add((String)r["APP_ID"]);
                    }
                }
                catch { }
                return roles;
            });
        }


        Task IUserRoleStore<MESUser, string>.RemoveFromRoleAsync(MESUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        Task<bool> IUserRoleStore<MESUser, string>.IsInRoleAsync(MESUser user, string roleName)
        {
            throw new NotImplementedException();
        }
        #endregion
        void IDisposable.Dispose()
        {
            db.Dispose();
        }
    }

}
