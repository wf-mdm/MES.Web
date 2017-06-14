using Intelli.MidW.BizClient;
using Intelli.MidW.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MES.Web.Controllers
{
    [Authorize]
    public class WarehouseController : Controller
    {
        // GET: Warehouse
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            DataSet ds = await Task<DataSet>.Run(() =>
            {
                BizRequest request = ClientMgr.Instance.CreateRequest("config", "MES", "", "GETUSRAPS", new Dictionary<string, string>(){
                    { "uid", HttpContext.User.Identity.Name},
                    { "modid", "MESWMS"}
                });
                request.UserId = HttpContext.User.Identity.Name;
                try
                {
                    return ClientMgr.Instance.RunDbCmd(request.CmdName, request);
                }
                catch (Exception ex)
                {
                }
                return null;
            });

            if (null != ds)
            {
                ViewBag.Features = ds.Tables["APP_MASTDATA"].Rows;
            }
            ViewBag.User = User.Identity.Name;
            return View();
        }
    }
}