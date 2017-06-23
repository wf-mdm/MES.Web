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
    [Authorize(Roles = "Ops")]
    public class OpsController : Controller
    {
        private static String ModelName = "工序";
        private MESDbContext db = new MESDbContext();

        // GET: Admin/Ops
        public async Task<ActionResult> Index(ENG_LINEOP Query)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "查询";
            ViewBag.Query = Query;
            ViewBag.LINENAME = new SelectList(db.ENG_PRDLINE, "LINENAME", "CodeName");
            var eNG_LINEOP = db.ENG_LINEOP.Include(e => e.Line);
            return View(await eNG_LINEOP
                .Where(l => String.IsNullOrEmpty(Query.LINENAME) || l.LINENAME.Equals(Query.LINENAME))
                .ToListAsync());
        }

        // GET: Admin/Ops/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ENG_LINEOP eNG_LINEOP = await db.ENG_LINEOP.FindAsync(id);
            if (eNG_LINEOP == null)
            {
                return HttpNotFound();
            }
            return View(eNG_LINEOP);
        }

        // GET: Admin/Ops/Create
        public ActionResult Create()
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "新建";

            ViewBag.LINENAME = new SelectList(db.ENG_PRDLINE, "LINENAME", "CodeName");
            return View();
        }

        // POST: Admin/Ops/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "LINENAME,L_OPNO,CodeName,OPDEFAULTSEQ,STDYIELDRATE,CYCLETIME")] ENG_LINEOP eNG_LINEOP)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "新建";
            if (ModelState.IsValid)
            {
                db.ENG_LINEOP.Add(eNG_LINEOP);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.LINENAME = new SelectList(db.ENG_PRDLINE, "LINENAME", "CodeName", eNG_LINEOP.LINENAME);
            return View(eNG_LINEOP);
        }

        // GET: Admin/Ops/Edit/5
        public async Task<ActionResult> Edit(String LINENAME,String L_OPNO)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "编辑";
            ENG_LINEOP eNG_LINEOP = await db.ENG_LINEOP.FindAsync(LINENAME, L_OPNO);
            if (eNG_LINEOP == null)
            {
                return HttpNotFound();
            }
            ViewBag.LINENAME = new SelectList(db.ENG_PRDLINE, "LINENAME", "CodeName", eNG_LINEOP.LINENAME);
            return View(eNG_LINEOP);
        }

        // POST: Admin/Ops/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "LINENAME,L_OPNO,DISPLAYNAME,OPDEFAULTSEQ,STDYIELDRATE,CYCLETIME")] ENG_LINEOP eNG_LINEOP)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "编辑";
            if (ModelState.IsValid)
            {
                db.Entry(eNG_LINEOP).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.LINENAME = new SelectList(db.ENG_PRDLINE, "LINENAME", "CodeName", eNG_LINEOP.LINENAME);
            return View(eNG_LINEOP);
        }

        // GET: Admin/Ops/Delete/5
        public async Task<ActionResult> Delete(String LINENAME, String L_OPNO)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "删除";
            ENG_LINEOP eNG_LINEOP = await db.ENG_LINEOP.FindAsync(LINENAME, L_OPNO);
            if (eNG_LINEOP == null)
            {
                return HttpNotFound();
            }
            return View(eNG_LINEOP);
        }

        // POST: Admin/Ops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(String LINENAME, String L_OPNO)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "删除";

            ENG_LINEOP eNG_LINEOP = await db.ENG_LINEOP.FindAsync(LINENAME, L_OPNO);
            db.ENG_LINEOP.Remove(eNG_LINEOP);
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
