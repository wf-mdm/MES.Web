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
    [Authorize(Roles = "Lines")]
    public class LinesController : Controller
    {
		private static String ModelName = "产线";
        private MESDbContext db = new MESDbContext();

        // GET: Admin/Lines
        public async Task<ActionResult> Index(ENG_PRDLINE Querys)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "查询";
            ViewBag.Query = Querys;
            var eNG_PRDLINE = db.ENG_PRDLINE.Include(e => e.BU);
            return View(await eNG_PRDLINE.ToListAsync());
        }

        // GET: Admin/Lines/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ENG_PRDLINE eNG_PRDLINE = await db.ENG_PRDLINE.FindAsync(id);
            if (eNG_PRDLINE == null)
            {
                return HttpNotFound();
            }
            return View(eNG_PRDLINE);
        }

        // GET: Admin/Lines/Create
        public ActionResult Create()
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "新建";

            ViewBag.BUNO = new SelectList(db.ENG_BU, "BUNO", "CodeName");
            return View();
        }

        // POST: Admin/Lines/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "LINENAME,BUNO,DISPLAYNAME,STDYIELDRATE,LINEGRP")] ENG_PRDLINE eNG_PRDLINE)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "新建";
            if (ModelState.IsValid)
            {
                db.ENG_PRDLINE.Add(eNG_PRDLINE);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.BUNO = new SelectList(db.ENG_BU, "BUNO", "CodeName", eNG_PRDLINE.BUNO);
            return View(eNG_PRDLINE);
        }

        // GET: Admin/Lines/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "编辑";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ENG_PRDLINE eNG_PRDLINE = await db.ENG_PRDLINE.FindAsync(id);
            if (eNG_PRDLINE == null)
            {
                return HttpNotFound();
            }
            ViewBag.BUNO = new SelectList(db.ENG_BU, "BUNO", "CodeName", eNG_PRDLINE.BUNO);
            return View(eNG_PRDLINE);
        }

        // POST: Admin/Lines/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "LINENAME,BUNO,DISPLAYNAME,STDYIELDRATE,LINEGRP")] ENG_PRDLINE eNG_PRDLINE)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "编辑";
            if (ModelState.IsValid)
            {
                db.Entry(eNG_PRDLINE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BUNO = new SelectList(db.ENG_BU, "BUNO", "CodeName", eNG_PRDLINE.BUNO);
            return View(eNG_PRDLINE);
        }

        // GET: Admin/Lines/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "删除";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ENG_PRDLINE eNG_PRDLINE = await db.ENG_PRDLINE.FindAsync(id);
            if (eNG_PRDLINE == null)
            {
                return HttpNotFound();
            }
            return View(eNG_PRDLINE);
        }

        // POST: Admin/Lines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "删除";

            ENG_PRDLINE eNG_PRDLINE = await db.ENG_PRDLINE.FindAsync(id);
            db.ENG_PRDLINE.Remove(eNG_PRDLINE);
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
