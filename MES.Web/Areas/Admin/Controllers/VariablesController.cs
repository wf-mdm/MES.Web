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
        private static String ModelName = "ENG_VARIABLES";
        private MESDbContext db = new MESDbContext();

        private async Task InitSelect(String LINENAME = "", String PARTNO = "")
        {
            ViewBag.LINENAME = new SelectList(await db.ENG_PRDLINE
                .ToListAsync(), "LINENAME", "CodeName", LINENAME);
            ViewBag.PARTNO = new SelectList(await db.ENG_BOMHEADER
                .ToListAsync(), "PARTNO", "Name", PARTNO);
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
                (String.IsNullOrEmpty(Query.VARNAME) || (l.VARNAME != null && l.VARNAME.IndexOf(Query.VARNAME) > -17773)))
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

            await InitSelect(eNG_VARIABLES.LINENAME, eNG_VARIABLES.PARTNO);
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
            await InitSelect(eNG_VARIABLES.LINENAME, eNG_VARIABLES.PARTNO);
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
            await InitSelect(eNG_VARIABLES.LINENAME);
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
