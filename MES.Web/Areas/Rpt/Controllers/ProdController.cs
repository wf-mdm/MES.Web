using MES.Web.Areas.Admin.Models;
using MES.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MES.Web.Areas.Rpt.Controllers
{
    public class ProdController : Controller
    {
        MESDbContext db = new MESDbContext();
        DBHelper db1 = new DBHelper();
        private static String ModelName = "生产统计";
        // GET: Rpt/Prod
        public async Task<ActionResult> Index()
        {
            ViewBag.Title = ModelName;
            ViewBag.ln = new SelectList(await db.ENG_PRDLINE.Where(e => "RUN".Equals(e.LINETYPE)).ToListAsync(), "LINENAME", "CodeName", "");
            ViewBag.subno = new SelectList(
                await db.ENG_ROUTE.Select(e => new SelectListItem()
                {
                    Value = e.SUBLINENO,
                    Text = e.SUBLINENO
                }).Distinct().ToListAsync(), "Value", "Text", "");
            ViewBag.stype = new SelectList(
                await db.ENG_CODECFG.Where(e => "WIPSUMTUNIT".Equals(e.CODENAME)).ToListAsync(), "CODEID", "Name", "");

            await Pns(null, null);
            return View();
        }

        public async Task<JsonResult> Pns(DateTime? dt1, DateTime? dt2)
        {
            String sql = "SELECT DISTINCT WO_ID, PARTNO FROM WIP_STATUS WHERE 1 = 1";
            int idx = 1;
            List<Object> args = new List<object>();
            if (dt1.HasValue)
            {
                sql += " AND RUNDT >= @1";
                idx++;
                args.Add(dt1.Value);
            }
            if (dt2.HasValue)
            {
                sql += " AND RUNDT >= @" + idx;
                args.Add(dt2.Value);
            }
            DataTable dt = await db1.QueryAsync(sql, args.ToArray());
            List<String> pns = new List<string>();
            List<String> wos = new List<string>();
            foreach (DataRow r in dt.Rows)
            {
                wos.Add((String)r["WO_ID"]);
                String pn = (String)r["PARTNO"];
                if (!pns.Contains(pn)) pns.Add(pn);
            };

            ViewBag.pns = new SelectList(pns.Select(e => new SelectListItem()
            {
                Value = e,
                Text = e
            }), "Value", "Text", "");

            ViewBag.wos = new SelectList(wos.Select(e => new SelectListItem()
            {
                Value = e,
                Text = e
            }), "Value", "Text", "");

            return Json(new { pns = ViewBag.pns, wos = ViewBag.wos });
        }
    }
}