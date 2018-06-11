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
    public class ContnrAttrsController : Controller
    {
		private static String ModelName = "WMS_CONTAINERTYPEATTR";
        private MESDbContext db = new MESDbContext();

        async Task Init(String CONTNRTYPE)
        {
            WMS_CONTNRTYPE ct = await db.WMS_CONTNRTYPE.Where(t => CONTNRTYPE.Equals(t.CONTNRTYPE)).FirstAsync();
            ViewBag.Title = ct.CodeName;
        }

        // GET: Admin/ContnrAttrs
        public async Task<ActionResult> Index(String CONTNRTYPE, WMS_CONTAINERTYPEATTR Query)
        {
            await Init(CONTNRTYPE);
            ViewBag.SubTitle = "查询";
            ViewBag.Query = Query;
            return View(await db.WMS_CONTAINERTYPEATTR
                .Where(a=>CONTNRTYPE.Equals(a.CONTNRTYPE)).ToListAsync());
        }

        // GET: Admin/ContnrAttrs/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WMS_CONTAINERTYPEATTR wMS_CONTAINERTYPEATTR = await db.WMS_CONTAINERTYPEATTR.FindAsync(id);
            if (wMS_CONTAINERTYPEATTR == null)
            {
                return HttpNotFound();
            }
            return View(wMS_CONTAINERTYPEATTR);
        }

        // GET: Admin/ContnrAttrs/Create
        public async Task<ActionResult> Create(String CONTNRTYPE)
        {
            await Init(CONTNRTYPE);
            ViewBag.SubTitle = "新建";
            ViewBag.CONTNRTYPE = CONTNRTYPE;
            return View();
        }

        // POST: Admin/ContnrAttrs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(String CONTNRTYPE,[Bind(Include = "CONTNRTYPE,PROPERTYNAME,PROPERTYVALUE")] WMS_CONTAINERTYPEATTR wMS_CONTAINERTYPEATTR)
        {
            await Init(CONTNRTYPE);
            ViewBag.SubTitle = "新建";
            ViewBag.CONTNRTYPE = CONTNRTYPE;
            wMS_CONTAINERTYPEATTR.CONTNRTYPE = CONTNRTYPE;
            if (ModelState.IsValid)
            {
                db.WMS_CONTAINERTYPEATTR.Add(wMS_CONTAINERTYPEATTR);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(wMS_CONTAINERTYPEATTR);
        }

        // GET: Admin/ContnrAttrs/Edit/5
        public async Task<ActionResult> Edit(String CONTNRTYPE, string id)
        {
            await Init(CONTNRTYPE);
            ViewBag.SubTitle = "编辑";
            ViewBag.CONTNRTYPE = CONTNRTYPE;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WMS_CONTAINERTYPEATTR wMS_CONTAINERTYPEATTR = await db.WMS_CONTAINERTYPEATTR.FindAsync(CONTNRTYPE, id);
            if (wMS_CONTAINERTYPEATTR == null)
            {
                return HttpNotFound();
            }
            return View(wMS_CONTAINERTYPEATTR);
        }

        // POST: Admin/ContnrAttrs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(String CONTNRTYPE, [Bind(Include = "CONTNRTYPE,PROPERTYNAME,PROPERTYVALUE")] WMS_CONTAINERTYPEATTR wMS_CONTAINERTYPEATTR)
        {
            await Init(CONTNRTYPE);
            ViewBag.SubTitle = "编辑";
            ViewBag.CONTNRTYPE = CONTNRTYPE;
            wMS_CONTAINERTYPEATTR.CONTNRTYPE = CONTNRTYPE;
            if (ModelState.IsValid)
            {
                db.Entry(wMS_CONTAINERTYPEATTR).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(wMS_CONTAINERTYPEATTR);
        }

        // GET: Admin/ContnrAttrs/Delete/5
        public async Task<ActionResult> Delete(String CONTNRTYPE, string id)
        {
            await Init(CONTNRTYPE);
            ViewBag.SubTitle = "删除";
            ViewBag.CONTNRTYPE = CONTNRTYPE;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WMS_CONTAINERTYPEATTR wMS_CONTAINERTYPEATTR = await db.WMS_CONTAINERTYPEATTR.FindAsync(CONTNRTYPE, id);
            if (wMS_CONTAINERTYPEATTR == null)
            {
                return HttpNotFound();
            }
            return View(wMS_CONTAINERTYPEATTR);
        }

        // POST: Admin/ContnrAttrs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(String CONTNRTYPE, string id)
        {
            await Init(CONTNRTYPE);
            ViewBag.SubTitle = "删除";
            ViewBag.CONTNRTYPE = CONTNRTYPE;

            WMS_CONTAINERTYPEATTR wMS_CONTAINERTYPEATTR = await db.WMS_CONTAINERTYPEATTR.FindAsync(CONTNRTYPE, id);
            db.WMS_CONTAINERTYPEATTR.Remove(wMS_CONTAINERTYPEATTR);
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
