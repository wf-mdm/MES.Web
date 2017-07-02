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
    [Authorize(Roles = "Params")]
    public class ParamsController : Controller
    {
        private static String ModelName = "工艺参数";
        private static String PARAM_TYPE = "PRD";
        private MESDbContext db = new MESDbContext();

        private ENG_LINEOPPARAMCONF Prepare(ENG_LINEOPPARAMCONF eNG_LINEOPPARAMCONF)
        {
            eNG_LINEOPPARAMCONF.PARAM_TYPE = PARAM_TYPE;
            if (String.IsNullOrEmpty(eNG_LINEOPPARAMCONF.LINENAME)) eNG_LINEOPPARAMCONF.LINENAME = "ALL";
            if (String.IsNullOrEmpty(eNG_LINEOPPARAMCONF.L_OPNO)) eNG_LINEOPPARAMCONF.L_OPNO = "ALL";
            if (String.IsNullOrEmpty(eNG_LINEOPPARAMCONF.L_STNO)) eNG_LINEOPPARAMCONF.L_STNO = "ALL";
            return eNG_LINEOPPARAMCONF;
        }

        private async Task InitSelect(String LINENAME, String OP = "", String STN = "", String DataType = "", String ParamType = "")
        {
            ViewBag.LINENAME = new SelectList(await db.ENG_PRDLINE
                .ToListAsync(), "LINENAME", "CodeName", LINENAME);
            if (String.IsNullOrEmpty(LINENAME))
            {
                ViewBag.L_OPNO = ViewBag.L_STNO = new SelectList(new List<ENG_LINEOP>());
            }
            else
            {
                ViewBag.L_OPNO = new SelectList(await db.ENG_LINEOP.Where(op => op.LINENAME.Equals(LINENAME)).ToListAsync()
                    , "L_OPNO", "CodeName", OP);
                if (String.IsNullOrEmpty(OP))
                {
                    ViewBag.L_STNO = new SelectList(new List<ENG_LINEOP>());
                }
                else
                {
                    ViewBag.L_STNO = new SelectList(await db.ENG_LINESTATION.Where(
                        s => s.LINENAME.Equals(LINENAME) && s.L_OPNO.Equals(OP)
                        ).ToListAsync(), "L_STNO", "CodeName", STN);
                }
            }

            ViewBag.DATA_TYPE = new SelectList(await db.ENG_CODECFG.Where(e => "ENGPARDTYPE".Equals(e.CODENAME)).ToListAsync(), "CODEID", "Name", DataType);
        }

        // GET: Admin/Params
        public async Task<ActionResult> Index(ENG_LINEOPPARAMCONF Query)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "查询";
            ViewBag.Query = Query;
            return View(await db.ENG_LINEOPPARAMCONF
                .Where((e =>
                    (String.IsNullOrEmpty(Query.CONFNAME) || Query.CONFNAME.Equals(e.CONFNAME))
                    && PARAM_TYPE.Equals(e.PARAM_TYPE)))
                .ToListAsync());
        }

        // GET: Admin/Params/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ENG_LINEOPPARAMCONF eNG_LINEOPPARAMCONF = await db.ENG_LINEOPPARAMCONF.FindAsync(id);
            if (eNG_LINEOPPARAMCONF == null)
            {
                return HttpNotFound();
            }
            return View(eNG_LINEOPPARAMCONF);
        }

        // GET: Admin/Params/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "新建";

            await InitSelect("");
            return View();
        }

        // POST: Admin/Params/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CONFNAME,CONFID,LINENAME,L_OPNO,L_STNO,PARAM_ID,PARAM_VAL,PARAM_TEXT,DATA_TYPE,COMMENTS")] ENG_LINEOPPARAMCONF eNG_LINEOPPARAMCONF)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "新建";
            Prepare(eNG_LINEOPPARAMCONF);
            if (ModelState.IsValid)
            {
                db.ENG_LINEOPPARAMCONF.Add(eNG_LINEOPPARAMCONF);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            await InitSelect(eNG_LINEOPPARAMCONF.LINENAME);
            return View(eNG_LINEOPPARAMCONF);
        }

        // GET: Admin/Params/Edit/5
        public async Task<ActionResult> Edit(string CONFNAME, decimal CONFID)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "编辑";
            ENG_LINEOPPARAMCONF eNG_LINEOPPARAMCONF = await db.ENG_LINEOPPARAMCONF.FindAsync(CONFNAME, CONFID);
            if (eNG_LINEOPPARAMCONF == null)
            {
                return HttpNotFound();
            }
            await InitSelect(eNG_LINEOPPARAMCONF.LINENAME, eNG_LINEOPPARAMCONF.L_OPNO, eNG_LINEOPPARAMCONF.L_STNO, eNG_LINEOPPARAMCONF.DATA_TYPE, eNG_LINEOPPARAMCONF.PARAM_TYPE);
            return View(eNG_LINEOPPARAMCONF);
        }

        // POST: Admin/Params/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CONFNAME,CONFID,LINENAME,L_OPNO,L_STNO,PARAM_ID,PARAM_VAL,PARAM_TEXT,DATA_TYPE,COMMENTS")] ENG_LINEOPPARAMCONF eNG_LINEOPPARAMCONF)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "编辑";
            Prepare(eNG_LINEOPPARAMCONF);
            if (ModelState.IsValid)
            {
                db.Entry(eNG_LINEOPPARAMCONF).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            await InitSelect(eNG_LINEOPPARAMCONF.LINENAME, eNG_LINEOPPARAMCONF.L_OPNO, eNG_LINEOPPARAMCONF.L_STNO, eNG_LINEOPPARAMCONF.DATA_TYPE, eNG_LINEOPPARAMCONF.PARAM_TYPE);
            return View(eNG_LINEOPPARAMCONF);
        }

        // GET: Admin/Params/Delete/5
        public async Task<ActionResult> Delete(string CONFNAME, decimal CONFID)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "删除";
            ENG_LINEOPPARAMCONF eNG_LINEOPPARAMCONF = await db.ENG_LINEOPPARAMCONF.FindAsync(CONFNAME, CONFID);
            if (eNG_LINEOPPARAMCONF == null)
            {
                return HttpNotFound();
            }
            return View(eNG_LINEOPPARAMCONF);
        }

        // POST: Admin/Params/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string CONFNAME, decimal CONFID)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "删除";

            ENG_LINEOPPARAMCONF eNG_LINEOPPARAMCONF = await db.ENG_LINEOPPARAMCONF.FindAsync(CONFNAME, CONFID);
            db.ENG_LINEOPPARAMCONF.Remove(eNG_LINEOPPARAMCONF);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<JsonResult> OPSTN(String LINENAME, String L_OPNO)
        {
            if (L_OPNO == null)
                return Json(new { OP = await db.ENG_LINEOP.Where(op => op.LINENAME.Equals(LINENAME)).ToListAsync() }, JsonRequestBehavior.AllowGet);

            else
                return Json(new { STN = await db.ENG_LINESTATION.Where(stn => stn.LINENAME.Equals(LINENAME) && stn.L_OPNO.Equals(L_OPNO)).ToListAsync() }, JsonRequestBehavior.AllowGet);
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
