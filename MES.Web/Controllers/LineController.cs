using Intelli.MidW.BizClient;
using Intelli.MidW.Interface;
using MES.Web.Models;
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
    public class LineController : Controller
    {
        MESDbContext db = new MESDbContext();

        public async Task<ActionResult> Index(String id)
        {
            String line = String.IsNullOrEmpty(id) ? (String.IsNullOrEmpty(Request.Url.Query) ? "" : Request.Url.Query.Substring(1)) : id;

            var lines = db.Lines.Where(l => String.IsNullOrEmpty(line) ? true : l.Name.Equals(line, StringComparison.OrdinalIgnoreCase));
            if (lines.Count() == 1)
                return await ShowLine(lines.Single());
            else
                return View("List", lines);
        }

        private async Task<ActionResult> ShowLine(LineModel line)
        {
            DataSet ds = await Task<DataSet>.Run(() =>
            {
                BizRequest request = ClientMgr.Instance.CreateRequest("config", "MES", "", "GETUSRAPS", new Dictionary<string, string>(){
                    { "uid", HttpContext.User.Identity.Name},
                    { "modid", "LINEADMIN"}
                });
                request.UserId = HttpContext.User.Identity.Name;
                try
                {
                    return ClientMgr.Instance.RunDbCmd(request.CmdName, request);
                }
                catch (Exception ex)
                {
                    ViewBag.Exception = ex;
                }
                return null;
            });

            if (null != ds)
            {
                ViewBag.Features = ds.Tables["APP_MASTDATA"].Rows;
                StringBuilder sb = new StringBuilder();
                foreach (var r in ViewBag.Features)
                {
                    sb.Append("#");
                    sb.Append(r["APP_ID"]);
                }
                sb.Append("#");
                ViewBag.FeatureStr = sb.ToString();
            }
            ViewBag.User = User.Identity.Name;

            return View(line);
        }

        public ActionResult Ops(String id)
        {
            return Json(db.Ops.Where(op => op.Line.Equals(id)));
        }
    }
}