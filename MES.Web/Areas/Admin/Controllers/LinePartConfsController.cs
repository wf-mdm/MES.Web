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
    public class LinePartConfsController : Controller
    {
		private static String ModelName = "生产线-料号匹配";
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

        // GET: Admin/LinePartConfs
        public async Task<ActionResult> Index(ENG_LINEPARTCONF Query)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "查询";
            ViewBag.Query = Query;
            await InitSelect(Query.LINENAME, Query.PARTNO);
            var eNG_LINEPARTCONF = db.ENG_LINEPARTCONF.Include(e => e.Line);
            return View(await eNG_LINEPARTCONF.ToListAsync());
        }

        // GET: Admin/LinePartConfs/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ENG_LINEPARTCONF eNG_LINEPARTCONF = await db.ENG_LINEPARTCONF.FindAsync(id);
            if (eNG_LINEPARTCONF == null)
            {
                return HttpNotFound();
            }
            return View(eNG_LINEPARTCONF);
        }

        // GET: Admin/LinePartConfs/Create
        public async Task<ActionResult> Create()
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "新建";

            await InitSelect("", "");
            return View();
        }

        // POST: Admin/LinePartConfs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "LINENAME,PARTNO,PARTVER,SETTINGType,STDYIELDRATE,CONTNRTYPE,RT_NAME,STDHEADCOUNTS,DEFAULT_CONFNAME")] ENG_LINEPARTCONF eNG_LINEPARTCONF)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "新建";
            if (ModelState.IsValid)
            {
                db.ENG_LINEPARTCONF.Add(eNG_LINEPARTCONF);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            await InitSelect(eNG_LINEPARTCONF.LINENAME, eNG_LINEPARTCONF.PARTNO);
            return View(eNG_LINEPARTCONF);
        }

        // GET: Admin/LinePartConfs/Edit/5
        public async Task<ActionResult> Edit(string LINENAME, String PARTNO)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "编辑";
            ENG_LINEPARTCONF eNG_LINEPARTCONF = await db.ENG_LINEPARTCONF.FindAsync(LINENAME, PARTNO);
            if (eNG_LINEPARTCONF == null)
            {
                return HttpNotFound();
            }
            await InitSelect(eNG_LINEPARTCONF.LINENAME, eNG_LINEPARTCONF.PARTNO);
            return View(eNG_LINEPARTCONF);
        }

        // POST: Admin/LinePartConfs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "LINENAME,PARTNO,PARTVER,SETTINGType,STDYIELDRATE,CONTNRTYPE,RT_NAME,STDHEADCOUNTS,DEFAULT_CONFNAME")] ENG_LINEPARTCONF eNG_LINEPARTCONF)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "编辑";
            if (ModelState.IsValid)
            {
                db.Entry(eNG_LINEPARTCONF).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            await InitSelect(eNG_LINEPARTCONF.LINENAME, eNG_LINEPARTCONF.PARTNO);
            return View(eNG_LINEPARTCONF);
        }

        // GET: Admin/LinePartConfs/Delete/5
        public async Task<ActionResult> Delete(string LINENAME, String PARTNO)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "删除";
            ENG_LINEPARTCONF eNG_LINEPARTCONF = await db.ENG_LINEPARTCONF.FindAsync(LINENAME, PARTNO);
            if (eNG_LINEPARTCONF == null)
            {
                return HttpNotFound();
            }
            return View(eNG_LINEPARTCONF);
        }

        // POST: Admin/LinePartConfs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string LINENAME, string PARTNO)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "删除";

            ENG_LINEPARTCONF eNG_LINEPARTCONF = await db.ENG_LINEPARTCONF.FindAsync(LINENAME, PARTNO);
            db.ENG_LINEPARTCONF.Remove(eNG_LINEPARTCONF);
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
