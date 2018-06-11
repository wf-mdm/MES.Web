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
    public class ContnrPartMapsController : Controller
    {
		private static String ModelName = "包装料号匹配";
        private MESDbContext db = new MESDbContext();

        public async Task InitSelect(String CONTNRTYPE, String PARTGRP2, String CONTRLBY, String SUBCONTNRTYPE)
        {
            ViewBag.PARTGRP2 = new SelectList(
                await db.WMS_PARTDATA
                    .Select(p => p.PARTGRPNO2)
                    .Distinct().ToListAsync(),
                PARTGRP2);
            ViewBag.CONTRLBY = new SelectList(
                await db.ENG_CODECFG
                    .Where(c => "CNTRCTYPCODE".Equals(c.CODENAME))
                    .Select(c => new SelectListItem()
                    {
                        Value = c.CODEID,
                        Text = c.CODEDESC
                    })
                    .Distinct().ToListAsync(),
                "Value", "Text", CONTRLBY);

            IList<WMS_CONTNRTYPE> WMS_CONTNRTYPEs = await db.WMS_CONTNRTYPE.ToListAsync();
            IList<SelectListItem> TYPES = WMS_CONTNRTYPEs
                .Select(t => new SelectListItem()
                {
                    Value = t.CONTNRTYPE,
                    Text = t.CodeName
                }).ToList();

            ViewBag.CONTNRTYPE = new SelectList(TYPES, "Value", "Text", CONTNRTYPE);
            ViewBag.SUBCONTNRTYPE = new SelectList(TYPES, "Value", "Text", SUBCONTNRTYPE);
        }

        // GET: Admin/ContnrPartMaps
        public async Task<ActionResult> Index(WMS_CONTNRPARTMAP Query)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "查询";
            ViewBag.Query = Query;
            return View(await db.WMS_CONTNRPARTMAP.ToListAsync());
        }

        // GET: Admin/ContnrPartMaps/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WMS_CONTNRPARTMAP wMS_CONTNRPARTMAP = await db.WMS_CONTNRPARTMAP.FindAsync(id);
            if (wMS_CONTNRPARTMAP == null)
            {
                return HttpNotFound();
            }
            return View(wMS_CONTNRPARTMAP);
        }

        // GET: Admin/ContnrPartMaps/Create
        public async Task<ActionResult> Create()
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "新建";

            await InitSelect("", "", "", "");

            return View();
        }

        // POST: Admin/ContnrPartMaps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CONTNRTYPE,PARTGRP2,VOLUMEQTY,VOL_UNIT,CONTAINSUBQTY,OVERLIMIT,CONTRLBY,SUBCONTNRTYPE")] WMS_CONTNRPARTMAP wMS_CONTNRPARTMAP)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "新建";
            if (ModelState.IsValid)
            {
                db.WMS_CONTNRPARTMAP.Add(wMS_CONTNRPARTMAP);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            await InitSelect(wMS_CONTNRPARTMAP.CONTNRTYPE, wMS_CONTNRPARTMAP.PARTGRP2, wMS_CONTNRPARTMAP.CONTRLBY, wMS_CONTNRPARTMAP.SUBCONTNRTYPE);
            return View(wMS_CONTNRPARTMAP);
        }

        // GET: Admin/ContnrPartMaps/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "编辑";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WMS_CONTNRPARTMAP wMS_CONTNRPARTMAP = await db.WMS_CONTNRPARTMAP.FindAsync(id);
            if (wMS_CONTNRPARTMAP == null)
            {
                return HttpNotFound();
            }
            await InitSelect(wMS_CONTNRPARTMAP.CONTNRTYPE, wMS_CONTNRPARTMAP.PARTGRP2, wMS_CONTNRPARTMAP.CONTRLBY, wMS_CONTNRPARTMAP.SUBCONTNRTYPE);
            return View(wMS_CONTNRPARTMAP);
        }

        // POST: Admin/ContnrPartMaps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CONTNRTYPE,PARTGRP2,VOLUMEQTY,VOL_UNIT,CONTAINSUBQTY,OVERLIMIT,CONTRLBY,SUBCONTNRTYPE")] WMS_CONTNRPARTMAP wMS_CONTNRPARTMAP)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "编辑";
            if (ModelState.IsValid)
            {
                db.Entry(wMS_CONTNRPARTMAP).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            await InitSelect(wMS_CONTNRPARTMAP.CONTNRTYPE, wMS_CONTNRPARTMAP.PARTGRP2, wMS_CONTNRPARTMAP.CONTRLBY, wMS_CONTNRPARTMAP.SUBCONTNRTYPE);
            return View(wMS_CONTNRPARTMAP);
        }

        // GET: Admin/ContnrPartMaps/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "删除";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WMS_CONTNRPARTMAP wMS_CONTNRPARTMAP = await db.WMS_CONTNRPARTMAP.FindAsync(id);
            if (wMS_CONTNRPARTMAP == null)
            {
                return HttpNotFound();
            }
            return View(wMS_CONTNRPARTMAP);
        }

        // POST: Admin/ContnrPartMaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "删除";

            WMS_CONTNRPARTMAP wMS_CONTNRPARTMAP = await db.WMS_CONTNRPARTMAP.FindAsync(id);
            db.WMS_CONTNRPARTMAP.Remove(wMS_CONTNRPARTMAP);
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
