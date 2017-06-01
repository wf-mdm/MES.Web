using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(MES.Web.Startup))]

namespace MES.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMes(app);
            ConfigureAuth(app);
        }
    }
}
