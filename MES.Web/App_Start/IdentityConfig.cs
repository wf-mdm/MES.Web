using System.Web;
using System.Web.Optimization;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using MES.Web.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace MES.Web
{
    public partial class Startup
    {
        public static void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext<MESUserManager>(()=>
            {
                return new MESUserManager();
            });
            app.CreatePerOwinContext<MESSignInManager>(MESSignInManager.Create);
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/app/Session"),
                //CookieHttpOnly = true,
                CookieName = "__MESSid"
            });
        }
    }

    public class MESSignInManager : SignInManager<MESUser, string>
    {
        public MESSignInManager(MESUserManager userManager, IAuthenticationManager authenticationManager) 
            : base(userManager, authenticationManager)
        {
        }
        public static MESSignInManager Create(IdentityFactoryOptions<MESSignInManager> options, IOwinContext context)
        {
            return new MESSignInManager(context.GetUserManager<MESUserManager>(), context.Authentication);
        }
    }
}
