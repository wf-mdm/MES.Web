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
    public class RoutesController : Controller
    {
		private static String ModelName = "路由";
        private MESDbContext db = new MESDbContext();

        // GET: Admin/Routes
        public async Task<ActionResult> Index(ENG_ROUTE Query)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "查询";
            ViewBag.Query = Query;
            var eNG_ROUTE = db.ENG_ROUTE.Include(e => e.Op);
            return View(await eNG_ROUTE
                .Where( r => (String.IsNullOrEmpty(Query.RT_NAME) || r.RT_NAME.Equals(Query.RT_NAME)))
                .ToListAsync());
        }

        // GET: Admin/Routes/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ENG_ROUTE eNG_ROUTE = await db.ENG_ROUTE.FindAsync(id);
            if (eNG_ROUTE == null)
            {
                return HttpNotFound();
            }
            return View(eNG_ROUTE);
        }

        // GET: Admin/Routes/Create
        public ActionResult Create()
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "新建";

            ViewBag.LINENAME = new SelectList(db.ENG_PRDLINE, "LINENAME", "CodeName");
            ViewBag.L_OPNO = new SelectList(new List<ENG_LINEOP>());
            return View();
        }

        // POST: Admin/Routes/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "RT_NAME,SEQNO,LINENAME,SUBLINENO,L_OPNO,PREV_OPLIST,NEXT_OPLIST,IsFirst,IsLast,COMMENTS")] ENG_ROUTE eNG_ROUTE)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "新建";
            if (ModelState.IsValid)
            {
                db.ENG_ROUTE.Add(eNG_ROUTE);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.LINENAME = new SelectList(db.ENG_LINEOP, "LINENAME", "CodeName", eNG_ROUTE.LINENAME);
            ViewBag.L_OPNO = new SelectList(await db.ENG_LINEOP
                .Where(op => op.LINENAME.Equals(eNG_ROUTE.LINENAME)).ToListAsync(), "L_OPNO", "CodeName", eNG_ROUTE.L_OPNO);
            return View(eNG_ROUTE);
        }

        // GET: Admin/Routes/Edit/5
        public async Task<ActionResult> Edit(string RT_NAME, decimal SEQNO)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "编辑";
            ENG_ROUTE eNG_ROUTE = await db.ENG_ROUTE.FindAsync(RT_NAME, SEQNO);
            if (eNG_ROUTE == null)
            {
                return HttpNotFound();
            }
            ViewBag.LINENAME = new SelectList(db.ENG_LINEOP, "LINENAME", "CodeName", eNG_ROUTE.LINENAME);
            ViewBag.L_OPNO = new SelectList(await db.ENG_LINEOP
                .Where(op => op.LINENAME.Equals(eNG_ROUTE.LINENAME)).ToListAsync(), "L_OPNO", "CodeName", eNG_ROUTE.L_OPNO);
            return View(eNG_ROUTE);
        }

        // POST: Admin/Routes/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RT_NAME,SEQNO,LINENAME,SUBLINENO,L_OPNO,PREV_OPLIST,NEXT_OPLIST,IsFirst,IsLast,COMMENTS")] ENG_ROUTE eNG_ROUTE)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "编辑";
            if (ModelState.IsValid)
            {
                db.Entry(eNG_ROUTE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.LINENAME = new SelectList(db.ENG_LINEOP, "LINENAME", "CodeName", eNG_ROUTE.LINENAME);
            ViewBag.L_OPNO = new SelectList(await db.ENG_LINEOP
                .Where(op => op.LINENAME.Equals(eNG_ROUTE.LINENAME)).ToListAsync(), "L_OPNO", "CodeName", eNG_ROUTE.L_OPNO);
            return View(eNG_ROUTE);
        }

        // GET: Admin/Routes/Delete/5
        public async Task<ActionResult> Delete(string RT_NAME, decimal SEQNO)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "删除";
            ENG_ROUTE eNG_ROUTE = await db.ENG_ROUTE.FindAsync(RT_NAME, SEQNO);
            if (eNG_ROUTE == null)
            {
                return HttpNotFound();
            }
            return View(eNG_ROUTE);
        }

        // POST: Admin/Routes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string RT_NAME, decimal SEQNO)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "删除";

            ENG_ROUTE eNG_ROUTE = await db.ENG_ROUTE.FindAsync(RT_NAME, SEQNO);
            db.ENG_ROUTE.Remove(eNG_ROUTE);
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
