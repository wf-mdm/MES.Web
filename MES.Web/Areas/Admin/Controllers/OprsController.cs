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
    public class OprsController : Controller
    {
        private static String ModelName = "HR_OPERATORS";
        private MESDbContext db = new MESDbContext();

        // GET: Admin/Oprs
        public async Task<ActionResult> Index(HR_OPERATORS Query)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "查询";
            ViewBag.Query = Query;
            var hR_OPERATORS = db.HR_OPERATORS.Include(h => h.BU);
            return View(await hR_OPERATORS.Where(o =>
                String.IsNullOrEmpty(Query.OPERNAME) || o.OPERNAME.IndexOf(Query.OPERNAME) > -1
            ).ToListAsync());
        }

        // GET: Admin/Oprs/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HR_OPERATORS hR_OPERATORS = await db.HR_OPERATORS.FindAsync(id);
            if (hR_OPERATORS == null)
            {
                return HttpNotFound();
            }
            return View(hR_OPERATORS);
        }

        // GET: Admin/Oprs/Create
        public ActionResult Create()
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "新建";

            ViewBag.BUNO = new SelectList(db.ENG_BU, "BUNO", "BUNAME");
            return View();
        }

        // POST: Admin/Oprs/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "OPERID,OPERNAME,PWD,BUNO,DEPTNO,ACTIVE,EMAIL,MOBILE,COMMENTS")] HR_OPERATORS hR_OPERATORS)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "新建";
            if (ModelState.IsValid)
            {
                db.HR_OPERATORS.Add(hR_OPERATORS);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.BUNO = new SelectList(db.ENG_BU, "BUNO", "BUNAME", hR_OPERATORS.BUNO);
            return View(hR_OPERATORS);
        }

        // GET: Admin/Oprs/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "编辑";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HR_OPERATORS hR_OPERATORS = await db.HR_OPERATORS.FindAsync(id);
            if (hR_OPERATORS == null)
            {
                return HttpNotFound();
            }
            ViewBag.BUNO = new SelectList(db.ENG_BU, "BUNO", "BUNAME", hR_OPERATORS.BUNO);
            return View(hR_OPERATORS);
        }

        // POST: Admin/Oprs/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "OPERID,OPERNAME,PWD,BUNO,DEPTNO,ACTIVE,EMAIL,MOBILE,COMMENTS")] HR_OPERATORS hR_OPERATORS)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "编辑";
            if (ModelState.IsValid)
            {
                db.Entry(hR_OPERATORS).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BUNO = new SelectList(db.ENG_BU, "BUNO", "BUNAME", hR_OPERATORS.BUNO);
            return View(hR_OPERATORS);
        }

        // GET: Admin/Oprs/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "删除";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HR_OPERATORS hR_OPERATORS = await db.HR_OPERATORS.FindAsync(id);
            if (hR_OPERATORS == null)
            {
                return HttpNotFound();
            }
            return View(hR_OPERATORS);
        }

        // POST: Admin/Oprs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "删除";

            HR_OPERATORS hR_OPERATORS = await db.HR_OPERATORS.FindAsync(id);
            db.HR_OPERATORS.Remove(hR_OPERATORS);
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
