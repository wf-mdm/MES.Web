using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MES.Web.Areas.Admin.Models;

namespace MES.Web.Areas.Admin.Controllers
{
    public class VariablesController : Controller
    {
        private static String ModelName = "工程变量";
        private MESDbContext db = new MESDbContext();

        private async Task InitSelect(String LINENAME = "", String PARTNO = "", String STN = "")
        {
            IList<ENG_PRDLINE> lines1 = await db.ENG_PRDLINE.ToListAsync();
            IList<SelectListItem> lines = lines1.Select(l => {
                return new SelectListItem()
                {
                    Value = l.LINENAME,
                    Text = l.CodeName
                };
            }).ToList();
            lines.Insert(0, new SelectListItem()
            {
                Value = "ALL",
                Text = "ALL"
            });

            ViewBag.LINENAME = new SelectList(lines, "Value", "Text", LINENAME);

            ViewBag.PARTNO = new SelectList(await db.WMS_PARTDATA
                .ToListAsync(), "PARTNO", "CodeName", PARTNO);

            List<ENG_LINESTATION> STNS = await db.ENG_LINESTATION.Where(
                s => s.LINENAME.Equals(LINENAME)).ToListAsync();
            STNS.Insert(0, new ENG_LINESTATION()
            {
                LINENAME = "ALL",
                L_STNO = "ALL"
            });
            ViewBag.L_STNO = new SelectList(STNS, "L_STNO", "CodeName", STN);
        }

        // GET: Admin/Variable
        public async Task<ActionResult> Index(ENG_VARIABLES Query)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "查询";
            ViewBag.Query = Query;
            await InitSelect(Query.LINENAME);
            return View(await db.ENG_VARIABLES
                .Where(l => (String.IsNullOrEmpty(Query.LINENAME) || l.LINENAME.Equals(Query.LINENAME)) &&
                (String.IsNullOrEmpty(Query.VARNAME) || (l.VARNAME != null && l.VARNAME.IndexOf(Query.VARNAME) > -1)))
                .ToListAsync());
        }

        // GET: Admin/Variable/Details/5
        public async Task<ActionResult> Details(String LINENAME, String PARTNO, String VARNAME)
        {
            ENG_VARIABLES eNG_VARIABLES = await db.ENG_VARIABLES.FindAsync(LINENAME, PARTNO, VARNAME);
            if (eNG_VARIABLES == null)
            {
                return HttpNotFound();
            }
            return View(eNG_VARIABLES);
        }

        // GET: Admin/Variable/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "新建";

            await InitSelect();
            return View();
        }

        // POST: Admin/Variable/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "LINENAME,PARTNO,VARNAME,VARTYPE,L_STNO,VARDESC,VARVALUE,UPDATETIME")] ENG_VARIABLES eNG_VARIABLES)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "新建";
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(eNG_VARIABLES.LINENAME)) eNG_VARIABLES.LINENAME = "ALL";
                db.ENG_VARIABLES.Add(eNG_VARIABLES);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            await InitSelect(eNG_VARIABLES.LINENAME, eNG_VARIABLES.PARTNO, eNG_VARIABLES.L_STNO);
            return View(eNG_VARIABLES);
        }

        // GET: Admin/Variable/Edit/5
        public async Task<ActionResult> Edit(String LINENAME, String PARTNO, String VARNAME)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "编辑";
            ENG_VARIABLES eNG_VARIABLES = await db.ENG_VARIABLES.FindAsync(LINENAME, PARTNO, VARNAME);
            if (eNG_VARIABLES == null)
            {
                return HttpNotFound();
            }
            await InitSelect(eNG_VARIABLES.LINENAME, eNG_VARIABLES.PARTNO, eNG_VARIABLES.L_STNO);
            return View(eNG_VARIABLES);
        }

        // POST: Admin/Variable/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "LINENAME,PARTNO,VARNAME,VARTYPE,L_STNO,VARDESC,VARVALUE,UPDATETIME")] ENG_VARIABLES eNG_VARIABLES)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "编辑";
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(eNG_VARIABLES.LINENAME)) eNG_VARIABLES.LINENAME = "ALL";
                if (String.IsNullOrEmpty(eNG_VARIABLES.PARTNO)) eNG_VARIABLES.PARTNO = "ALL";
                db.Entry(eNG_VARIABLES).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            await InitSelect(eNG_VARIABLES.LINENAME, eNG_VARIABLES.PARTNO, eNG_VARIABLES.L_STNO);
            return View(eNG_VARIABLES);
        }

        // GET: Admin/Variable/Delete/5
        public async Task<ActionResult> Delete(String LINENAME, String PARTNO, String VARNAME)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "删除";
            ENG_VARIABLES eNG_VARIABLES = await db.ENG_VARIABLES.FindAsync(LINENAME, PARTNO, VARNAME);
            if (eNG_VARIABLES == null)
            {
                return HttpNotFound();
            }
            return View(eNG_VARIABLES);
        }

        // POST: Admin/Variable/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(String LINENAME, String PARTNO, String VARNAME)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "删除";

            ENG_VARIABLES eNG_VARIABLES = await db.ENG_VARIABLES.FindAsync(LINENAME, PARTNO, VARNAME);
            db.ENG_VARIABLES.Remove(eNG_VARIABLES);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<JsonResult> OPSTN(String LINENAME)
        {
            List< ENG_LINESTATION>  STNS = await db.ENG_LINESTATION.Where(stn => stn.LINENAME.Equals(LINENAME)).ToListAsync();
            STNS.Insert(0, new ENG_LINESTATION()
            {
                LINENAME = "ALL",
                L_STNO = "ALL"
            });
            return Json(new
            {
                STN = STNS
            }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
