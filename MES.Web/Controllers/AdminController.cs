using Intelli.MidW.BizClient;
using Intelli.MidW.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MES.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public object BzRequest { get; private set; }

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Menu()
        {
            BizRequest request = ClientMgr.Instance.CreateRequest("config", "MES", "", "GETUSRAPS", new Dictionary<string, string>()
            {
                { "uid", HttpContext.User.Identity.Name},
                { "modid", "MESADMIN"}
            });
            request.UserId = HttpContext.User.Identity.Name;

            try
            {
                DataSet ds = ClientMgr.Instance.RunDbCmd(request.CmdName, request);

                return PartialView(ds);
            }catch(Exception ex)
            {
                return PartialView("MenuError");
            }

        }
    }
}