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
using System.Web.Script.Serialization;

namespace MES.Web.Controllers
{
    [Authorize]
    public class StnController : Controller
    {
        MESDbContext db = new MESDbContext();

        public async Task<ActionResult> Index(String id)
        {
            String str = String.IsNullOrEmpty(id) ? (String.IsNullOrEmpty(Request.Url.Query) ? "" : Request.Url.Query.Substring(1)) : id;
            var strs = str.Split(';');
            String line = strs[0];
            String op = strs.Length > 1 ? strs[1] : null;
            String stn = strs.Length > 2 ? strs[2] : null;

            var model = db.Stns.Where(s =>
                s.Line.Equals(line, StringComparison.OrdinalIgnoreCase)
                && s.Op.Equals(op, StringComparison.OrdinalIgnoreCase)
                && s.Stn.Equals(stn, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
            if (model != null)
                return await ShowStn(model);
            else
                return View("Error", str);
        }

        private async Task<ActionResult> ShowStn(StnModel stn)
        {
            ViewBag.Line = db.Lines.Where(l => l.Name.Equals(stn.Line)).SingleOrDefault();

            DataSet ds = await Task<DataSet>.Run(() =>
            {
                BizRequest request = ClientMgr.Instance.CreateRequest("config", "MES", "", "GETUSRAPS", new Dictionary<string, string>(){
                    { "uid", HttpContext.User.Identity.Name},
                    { "modid", "STADMIN"}
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
                Dictionary<object, object> Features = new Dictionary<object, object>();
                foreach (DataRow r in ds.Tables["APP_MASTDATA"].Rows)
                {
                    Features[r["APP_ID"]] = new { p = r["APP_PATH"], n = r["APP_DESCRIPTION"] };
                }
                JavaScriptSerializer jss = new JavaScriptSerializer();
                ViewBag.FeatureStr = jss.Serialize(Features);
            }
            ViewBag.User = User.Identity.Name;

            return View(stn);
        }
    }
}