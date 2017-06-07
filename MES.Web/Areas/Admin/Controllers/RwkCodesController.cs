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
    //[Authorize(Roles = "RwkCodes")]
    public class RwkCodesController : Controller
    {
        private static String ModelName = "质量代码";
        private MESDbContext db = new MESDbContext();

        private async Task InitSelect(String QYTYPE, String LINENAME, String OP1 = "", String OP2 = "")
        {
            ViewBag.QCTYPE = new SelectList(await db.ENG_CODECFG.Where(c => "WIPQCTYPE".Equals(c.CODENAME)).ToListAsync(),
                "CODEID", "NAME", QYTYPE);
            ViewBag.LINENAME = new SelectList(await db.ENG_PRDLINE
                .ToListAsync(), "LINENAME", "CodeName", LINENAME);
            if (String.IsNullOrEmpty(LINENAME))
            {
                ViewBag.DEFAULTOOP = ViewBag.FROMOP = new SelectList(new List<ENG_LINEOP>());
            }
            else
            {
                List<ENG_LINEOP> ops = await db.ENG_LINEOP.Where(op => op.LINENAME.Equals(LINENAME)).ToListAsync();
                ViewBag.DEFAULTOOP = new SelectList(ops, "L_OPNO", "CodeName", OP1);
                ViewBag.FROMOP = new SelectList(ops, "L_OPNO", "CodeName", OP2);
            }
        }

        // GET: Admin/RwkCodes
        public async Task<ActionResult> Index(ENG_RWKSCRCODE Query)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "查询";
            ViewBag.Query = Query;
            await InitSelect(Query.QCCODE, Query.LINENAME);
            return View(await db.ENG_RWKSCRCODE
                                .Where(code =>
                    (String.IsNullOrEmpty(Query.QCTYPE) || code.QCTYPE.Equals(Query.QCTYPE))
                    && (String.IsNullOrEmpty(Query.LINENAME) || code.LINENAME.Equals(Query.LINENAME))
                ).ToListAsync());
        }

        // GET: Admin/RwkCodes/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ENG_RWKSCRCODE eNG_RWKSCRCODE = await db.ENG_RWKSCRCODE.FindAsync(id);
            if (eNG_RWKSCRCODE == null)
            {
                return HttpNotFound();
            }
            return View(eNG_RWKSCRCODE);
        }

        // GET: Admin/RwkCodes/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "新建";

            await InitSelect(null, null);
            return View();
        }

        // POST: Admin/RwkCodes/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "QCCODE,QCTYPE,DESCRIPTION,LINENAME,DEFAULTOOP,FROMOP,COMMENTS")] ENG_RWKSCRCODE eNG_RWKSCRCODE)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "新建";
            if (ModelState.IsValid)
            {
                db.ENG_RWKSCRCODE.Add(eNG_RWKSCRCODE);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            await InitSelect(eNG_RWKSCRCODE.QCCODE, eNG_RWKSCRCODE.LINENAME);
            return View(eNG_RWKSCRCODE);
        }

        // GET: Admin/RwkCodes/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "编辑";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ENG_RWKSCRCODE eNG_RWKSCRCODE = await db.ENG_RWKSCRCODE.FindAsync(id);
            if (eNG_RWKSCRCODE == null)
            {
                return HttpNotFound();
            }
            await InitSelect(eNG_RWKSCRCODE.QCCODE, eNG_RWKSCRCODE.LINENAME);
            return View(eNG_RWKSCRCODE);
        }

        // POST: Admin/RwkCodes/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "QCCODE,QCTYPE,DESCRIPTION,LINENAME,DEFAULTOOP,FROMOP,COMMENTS")] ENG_RWKSCRCODE eNG_RWKSCRCODE)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "编辑";
            if (ModelState.IsValid)
            {
                db.Entry(eNG_RWKSCRCODE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            await InitSelect(eNG_RWKSCRCODE.QCCODE, eNG_RWKSCRCODE.LINENAME);
            return View(eNG_RWKSCRCODE);
        }

        // GET: Admin/RwkCodes/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "删除";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ENG_RWKSCRCODE eNG_RWKSCRCODE = await db.ENG_RWKSCRCODE.FindAsync(id);
            if (eNG_RWKSCRCODE == null)
            {
                return HttpNotFound();
            }
            return View(eNG_RWKSCRCODE);
        }

        // POST: Admin/RwkCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "删除";

            ENG_RWKSCRCODE eNG_RWKSCRCODE = await db.ENG_RWKSCRCODE.FindAsync(id);
            db.ENG_RWKSCRCODE.Remove(eNG_RWKSCRCODE);
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
