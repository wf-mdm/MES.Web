using MES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
                return View(lines.Single());
            else
                return View("List", lines);
        }

        public ActionResult List()
        {
            return View();
        }
    }
}