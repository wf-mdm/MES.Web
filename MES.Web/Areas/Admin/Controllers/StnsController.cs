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
    [Authorize(Roles = "Stns")]
    public class StnsController : Controller
    {
		private static String ModelName = "工站";
        private MESDbContext db = new MESDbContext();

        // GET: Admin/Stns
        public async Task<ActionResult> Index(ENG_LINESTATION Query)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "查询";
            ViewBag.Query = Query;
            ViewBag.LINENAME = new SelectList(db.ENG_PRDLINE, "LINENAME", "CodeName");
            var eNG_LINESTATION = db.ENG_LINESTATION.Include(e => e.Op);
            return View(await eNG_LINESTATION
                .Where(stn => String.IsNullOrEmpty(Query.LINENAME) || stn.LINENAME.Equals(Query.LINENAME))
                .ToListAsync());
        }

        // GET: Admin/Stns/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ENG_LINESTATION eNG_LINESTATION = await db.ENG_LINESTATION.FindAsync(id);
            if (eNG_LINESTATION == null)
            {
                return HttpNotFound();
            }
            return View(eNG_LINESTATION);
        }

        // GET: Admin/Stns/Create
        public ActionResult Create()
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "新建";

            ViewBag.LINENAME = new SelectList(db.ENG_PRDLINE, "LINENAME", "CodeName");
            ViewBag.L_OPNO = new SelectList(new List<ENG_LINEOP>());
            return View();
        }

        // POST: Admin/Stns/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "LINENAME,L_STNO,L_OPNO,DISPLAYNAME")] ENG_LINESTATION eNG_LINESTATION)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "新建";
            if (ModelState.IsValid)
            {
                db.ENG_LINESTATION.Add(eNG_LINESTATION);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.LINENAME = new SelectList(db.ENG_LINEOP, "LINENAME", "CodeName", eNG_LINESTATION.LINENAME);
            ViewBag.L_OPNO = new SelectList(await db.ENG_LINEOP
                .Where(op=> op.LINENAME.Equals(eNG_LINESTATION.LINENAME)).ToListAsync(), "L_OPNO", "CodeName", eNG_LINESTATION.L_OPNO);
            return View(eNG_LINESTATION);
        }

        // GET: Admin/Stns/Edit/5
        public async Task<ActionResult> Edit(String LINENAME, String L_STNO)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "编辑";
            ENG_LINESTATION eNG_LINESTATION = await db.ENG_LINESTATION.FindAsync(LINENAME, L_STNO);
            if (eNG_LINESTATION == null)
            {
                return HttpNotFound();
            }
            ViewBag.LINENAME = new SelectList(db.ENG_LINEOP, "LINENAME", "CodeName", eNG_LINESTATION.LINENAME);
            ViewBag.L_OPNO = new SelectList(await db.ENG_LINEOP
                .Where(op => op.LINENAME.Equals(eNG_LINESTATION.LINENAME)).ToListAsync(), "L_OPNO", "CodeName", eNG_LINESTATION.L_OPNO);
            return View(eNG_LINESTATION);
        }

        // POST: Admin/Stns/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "LINENAME,L_STNO,L_OPNO, DISPLAYNAME")] ENG_LINESTATION eNG_LINESTATION)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "编辑";
            if (ModelState.IsValid)
            {
                db.Entry(eNG_LINESTATION).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.LINENAME = new SelectList(db.ENG_LINEOP, "LINENAME", "CodeName", eNG_LINESTATION.LINENAME);
            ViewBag.L_OPNO = new SelectList(await db.ENG_LINEOP
                .Where(op => op.LINENAME.Equals(eNG_LINESTATION.LINENAME)).ToListAsync(), "L_OPNO", "CodeName", eNG_LINESTATION.L_OPNO);
            return View(eNG_LINESTATION);
        }

        // GET: Admin/Stns/Delete/5
        public async Task<ActionResult> Delete(String LINENAME, String L_STNO)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "删除";
            ENG_LINESTATION eNG_LINESTATION = await db.ENG_LINESTATION.FindAsync(LINENAME, L_STNO);
            if (eNG_LINESTATION == null)
            {
                return HttpNotFound();
            }
            return View(eNG_LINESTATION);
        }

        // POST: Admin/Stns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(String LINENAME, String L_STNO)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "删除";

            ENG_LINESTATION eNG_LINESTATION = await db.ENG_LINESTATION.FindAsync(LINENAME, L_STNO);
            db.ENG_LINESTATION.Remove(eNG_LINESTATION);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<JsonResult> OPSTN(String LINENAME)
        {
            return Json(new
            {
                OP = await db.ENG_LINEOP.Where(op => op.LINENAME.Equals(LINENAME)).ToListAsync()
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
