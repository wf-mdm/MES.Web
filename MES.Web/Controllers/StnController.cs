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
    public class StnController : Controller
    {
        MESDbContext db = new MESDbContext();

        public ActionResult Index(String id)
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
                return ShowStn(model);
            else
                return View("Error", str);
        }

        private ActionResult ShowStn(StnModel stn)
        {
            ViewBag.Line = db.Lines.Where(l => l.Name.Equals(stn.Line)).SingleOrDefault();
            return View(stn);
        }
    }
}