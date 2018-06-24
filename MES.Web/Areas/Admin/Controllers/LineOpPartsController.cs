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
    public class LineOpPartsController : Controller
    {
		private static String ModelName = "工序工时配置";
        private MESDbContext db = new MESDbContext();

        public async Task InitSelect(String LINENAME, String PARTNO)
        {
            IList<ENG_PRDLINE> lines1 = await db.ENG_PRDLINE.ToListAsync();
            IList<SelectListItem> lines = lines1.Select(l => {
                return new SelectListItem()
                {
                    Value = l.LINENAME,
                    Text = l.CodeName
                };
            }).ToList();
            ViewBag.LINENAME = new SelectList(lines, "Value", "Text", LINENAME);

            IList<WMS_PARTDATA> pns1 = await db.WMS_PARTDATA.ToListAsync();
            IList<SelectListItem> pns = pns1.Select(pn => {
                return new SelectListItem()
                {
                    Value = pn.PARTNO,
                    Text = pn.CodeName
                };
            }).ToList();
            ViewBag.PARTNO = new SelectList(pns, "Value", "Text", LINENAME);
        }

        // GET: Admin/LineOpParts
        public async Task<ActionResult> Index(ENG_LINEOPPARTCONF Query)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "查询";
            ViewBag.Query = Query;
            return View(await db.ENG_LINEOPPARTCONF.ToListAsync());
        }

        // GET: Admin/LineOpParts/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ENG_LINEOPPARTCONF eNG_LINEOPPARTCONF = await db.ENG_LINEOPPARTCONF.FindAsync(id);
            if (eNG_LINEOPPARTCONF == null)
            {
                return HttpNotFound();
            }
            return View(eNG_LINEOPPARTCONF);
        }

        // GET: Admin/LineOpParts/Create
        public async Task<ActionResult> Create()
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "新建";

            await InitSelect("", "");
            ENG_LINEOPPARTCONF eNG_LINEOPPARTCONF = new ENG_LINEOPPARTCONF();
            return View(eNG_LINEOPPARTCONF);
        }

        // POST: Admin/LineOpParts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "LINENAME,PARTNO,SETTINGType,L_OPNO,L_STNO,STDYIELDRATE,PREPARETIME,POSTTIME,CYCLETIME,STDHEADS,IDCFGLIST,COMMENT,NGIDUNBINDLST,PROPLIST")] ENG_LINEOPPARTCONF eNG_LINEOPPARTCONF)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "新建";
            if (ModelState.IsValid)
            {
                db.ENG_LINEOPPARTCONF.Add(eNG_LINEOPPARTCONF);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            await InitSelect(eNG_LINEOPPARTCONF.LINENAME, eNG_LINEOPPARTCONF.PARTNO);
            return View(eNG_LINEOPPARTCONF);
        }

        // GET: Admin/LineOpParts/Edit/5
        public async Task<ActionResult> Edit(string LINENAME, String PARTNO)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "编辑";
            ENG_LINEOPPARTCONF eNG_LINEOPPARTCONF = await db.ENG_LINEOPPARTCONF.FindAsync(LINENAME, PARTNO);
            if (eNG_LINEOPPARTCONF == null)
            {
                return HttpNotFound();
            }
            await InitSelect(eNG_LINEOPPARTCONF.LINENAME, eNG_LINEOPPARTCONF.PARTNO);
            return View(eNG_LINEOPPARTCONF);
        }

        // POST: Admin/LineOpParts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "LINENAME,PARTNO,SETTINGType,L_OPNO,L_STNO,STDYIELDRATE,PREPARETIME,POSTTIME,CYCLETIME,STDHEADS,IDCFGLIST,COMMENT,NGIDUNBINDLST,PROPLIST")] ENG_LINEOPPARTCONF eNG_LINEOPPARTCONF)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "编辑";
            if (ModelState.IsValid)
            {
                db.Entry(eNG_LINEOPPARTCONF).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            await InitSelect(eNG_LINEOPPARTCONF.LINENAME, eNG_LINEOPPARTCONF.PARTNO);
            return View(eNG_LINEOPPARTCONF);
        }

        // GET: Admin/LineOpParts/Delete/5
        public async Task<ActionResult> Delete(string LINENAME, string PARTNO)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "删除";
            ENG_LINEOPPARTCONF eNG_LINEOPPARTCONF = await db.ENG_LINEOPPARTCONF.FindAsync(LINENAME, PARTNO);
            if (eNG_LINEOPPARTCONF == null)
            {
                return HttpNotFound();
            }
            await InitSelect(eNG_LINEOPPARTCONF.LINENAME, eNG_LINEOPPARTCONF.PARTNO);
            return View(eNG_LINEOPPARTCONF);
        }

        // POST: Admin/LineOpParts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string LINENAME, string PARTNO)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "删除";

            ENG_LINEOPPARTCONF eNG_LINEOPPARTCONF = await db.ENG_LINEOPPARTCONF.FindAsync(LINENAME, PARTNO);
            db.ENG_LINEOPPARTCONF.Remove(eNG_LINEOPPARTCONF);
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
