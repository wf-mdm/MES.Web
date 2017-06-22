using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MES.Web.Areas.Rpt.Controllers
{
    [Authorize]
    public class TraceabilityController : Controller
    {
        private static String ModelName = "追溯查询";
        // GET: Rpt/Traceability
        public ActionResult Index()
        {
            ViewBag.Title = ModelName;
            return View();
        }
    }
}