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

        public ActionResult Index(String id)
        {
            String line = String.IsNullOrEmpty(id) ? (String.IsNullOrEmpty(Request.Url.Query) ? "" : Request.Url.Query.Substring(1)) : id;

            var lines = db.Lines.Where(l => String.IsNullOrEmpty(line) ? true : l.Name.Equals(line, StringComparison.OrdinalIgnoreCase));
            if (lines.Count() == 1)
                return ShowLine(lines.Single());
            else
                return View("List", lines);
        }

        private ActionResult ShowLine(LineModel line)
        {
            BizRequest req = ClientMgr.Instance.CreateRequest("mes", line.Name, line.Name, "GETUSRLNFNS", new Dictionary<String, String> {
                { "uid", User.Identity.Name}
            });
            req.UserId = User.Identity.Name;
            DataSet ds = ClientMgr.Instance.RunDbCmd(req.CmdName, req);

            ViewBag.Features = ds.Tables["APP_MASTDATA"].Rows;
            StringBuilder sb = new StringBuilder();
            foreach(var r in ViewBag.Features)
            {
                sb.Append("#");
                sb.Append(r["APP_ID"]);
            }
            sb.Append("#");
            ViewBag.FeatureStr = sb.ToString();
            ViewBag.User = User.Identity.Name;

            return View(line);
        }
    }
}