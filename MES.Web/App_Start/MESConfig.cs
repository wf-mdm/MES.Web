using System.Web;
using System.Web.Optimization;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using MES.Web.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Intelli.MidW.BizClient;

namespace MES.Web
{
    public partial class Startup
    {
        public static void ConfigureMes(IAppBuilder app)
        {
            new ClientMgr().Init();
        }
    }
}
