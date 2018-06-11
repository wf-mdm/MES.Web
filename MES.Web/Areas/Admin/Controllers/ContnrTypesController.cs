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
    public class ContnrTypesController : Controller
    {
        private static String ModelName = "包装类型";
        private MESDbContext db = new MESDbContext();

        public async Task InitSelect(String PARTGRP2, String CONTRLBY, String SUBCONTNRTYPE, String PARENTTYPE)
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

            IList< WMS_CONTNRTYPE> WMS_CONTNRTYPEs = await db.WMS_CONTNRTYPE.ToListAsync();
            IList<SelectListItem> TYPES = WMS_CONTNRTYPEs
                .Select(t => new SelectListItem()
                {
                    Value = t.CONTNRTYPE,
                    Text = t.CodeName
                }).ToList();

            ViewBag.SUBCONTNRTYPE = new SelectList(TYPES, "Value", "Text", SUBCONTNRTYPE);
            ViewBag.PARENTTYPE = new SelectList(TYPES, "Value", "Text", PARENTTYPE);
        }

        // GET: Admin/ContnrTypes
        public async Task<ActionResult> Index(WMS_CONTNRTYPE Query)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "查询";
            ViewBag.Query = Query;
            await InitSelect("", "", "", "");
            return View(await db.WMS_CONTNRTYPE.ToListAsync());
        }

        // GET: Admin/ContnrTypes/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WMS_CONTNRTYPE wMS_CONTNRTYPE = await db.WMS_CONTNRTYPE.FindAsync(id);
            if (wMS_CONTNRTYPE == null)
            {
                return HttpNotFound();
            }
            return View(wMS_CONTNRTYPE);
        }

        // GET: Admin/ContnrTypes/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "新建";

            await InitSelect("", "", "", "");
            return View();
        }

        // POST: Admin/ContnrTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CONTNRTYPE,CONTAINERDESC,PARTGRP2,VOLUMEQTY,VOL_UNIT,CONTAINSUBQTY,OVERLIMIT,CONTRLBY,SUBCONTNRTYPE,PARENTTYPE,SINGLEPART,COMMENTS")] WMS_CONTNRTYPE wMS_CONTNRTYPE)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "新建";
            if (ModelState.IsValid)
            {
                db.WMS_CONTNRTYPE.Add(wMS_CONTNRTYPE);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            await InitSelect(wMS_CONTNRTYPE.PARTGRP2, wMS_CONTNRTYPE.CONTRLBY, wMS_CONTNRTYPE.SUBCONTNRTYPE, wMS_CONTNRTYPE.PARENTTYPE);
            return View(wMS_CONTNRTYPE);
        }

        // GET: Admin/ContnrTypes/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "编辑";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WMS_CONTNRTYPE wMS_CONTNRTYPE = await db.WMS_CONTNRTYPE.FindAsync(id);
            if (wMS_CONTNRTYPE == null)
            {
                return HttpNotFound();
            }
            await InitSelect(wMS_CONTNRTYPE.PARTGRP2, wMS_CONTNRTYPE.CONTRLBY, wMS_CONTNRTYPE.SUBCONTNRTYPE, wMS_CONTNRTYPE.PARENTTYPE);
            return View(wMS_CONTNRTYPE);
        }

        // POST: Admin/ContnrTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CONTNRTYPE,CONTAINERDESC,PARTGRP2,VOLUMEQTY,VOL_UNIT,CONTAINSUBQTY,OVERLIMIT,CONTRLBY,SUBCONTNRTYPE,PARENTTYPE,SINGLEPART,COMMENTS")] WMS_CONTNRTYPE wMS_CONTNRTYPE)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "编辑";
            if (ModelState.IsValid)
            {
                db.Entry(wMS_CONTNRTYPE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            await InitSelect(wMS_CONTNRTYPE.PARTGRP2, wMS_CONTNRTYPE.CONTRLBY, wMS_CONTNRTYPE.SUBCONTNRTYPE, wMS_CONTNRTYPE.PARENTTYPE);
            return View(wMS_CONTNRTYPE);
        }

        // GET: Admin/ContnrTypes/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "删除";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WMS_CONTNRTYPE wMS_CONTNRTYPE = await db.WMS_CONTNRTYPE.FindAsync(id);
            if (wMS_CONTNRTYPE == null)
            {
                return HttpNotFound();
            }
            return View(wMS_CONTNRTYPE);
        }

        // POST: Admin/ContnrTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "删除";

            WMS_CONTNRTYPE wMS_CONTNRTYPE = await db.WMS_CONTNRTYPE.FindAsync(id);
            db.WMS_CONTNRTYPE.Remove(wMS_CONTNRTYPE);
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
