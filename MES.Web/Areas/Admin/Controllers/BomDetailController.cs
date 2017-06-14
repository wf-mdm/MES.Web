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
    [Authorize(Roles = "Boms")]
    public class BomDetailController : Controller
    {
        private static String ModelName = "BOM";
        private MESDbContext db = new MESDbContext();

        private async Task InitSelect(String GRP)
        {
            IList<SelectListItem> grps = await db.ENG_PRDLINE.Select(e => new SelectListItem()
            {
                Text = e.LINEGRP,
                Value = e.LINEGRP
            }).Distinct().ToListAsync();
            ViewBag.SEMILINEGRP = new SelectList(grps, "Value", "Text", GRP);
        }

        // GET: Admin/BomDetail
        public async Task<ActionResult> Index(String PN, String VER, ENG_BOMDETAIL Query)
        {
            if (String.IsNullOrEmpty(PN) || String.IsNullOrEmpty(VER))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Title = String.Format("物料{0}@{1}", PN, VER);
            ViewBag.SubTitle = "查询";
            ViewBag.Query = Query;
            ViewBag.PN = PN;
            ViewBag.VER = VER;
            ViewBag.PATH = String.Format("/{0}.{1}/", PN, VER);
            return View(await db.ENG_BOMDETAIL.Where(d =>
                PN.Equals(d.PARTNO) && d.PARTVER.Equals(d.PARTVER)
            ).ToListAsync());
        }

        // GET: Admin/BomDetail/Create
        public async Task<ActionResult> Create(String PN, String VER)
        {
            if (String.IsNullOrEmpty(PN) || String.IsNullOrEmpty(VER))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Title = String.Format("物料{0}@{1}", PN, VER);
            ViewBag.SubTitle = "新建";
            ViewBag.PN = PN;
            ViewBag.VER = VER;
            ViewBag.PATH = String.Format("/{0}.{1}/", PN, VER);

            ENG_BOMDETAIL eNG_BOMDETAIL = new ENG_BOMDETAIL()
            {
                PARTNO = PN,
                PARTVER = VER
            };
            await InitSelect("");
            return View(eNG_BOMDETAIL);
        }

        // POST: Admin/BomDetail/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(String PN, String VER, [Bind(Include = "ITEMID,PARTNO,PARTVER,L_OPNO,COMP_PARTNO,DESCRIPTION,UNITCONSUMEQTY,LotControl,SERIALCONTROL,IsKeyID,CNTCONTROL,ISSEMI,SEMILINEGRP")] ENG_BOMDETAIL eNG_BOMDETAIL)
        {
            if (String.IsNullOrEmpty(PN) || String.IsNullOrEmpty(VER))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Title = String.Format("物料{0}@{1}", PN, VER);
            ViewBag.SubTitle = "新建";
            ViewBag.PN = PN;
            ViewBag.VER = VER;
            ViewBag.PATH = String.Format("/{0}.{1}/", PN, VER);

            if (ModelState.IsValid)
            {
                db.ENG_BOMDETAIL.Add(eNG_BOMDETAIL);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            await InitSelect(eNG_BOMDETAIL.SEMILINEGRP);
            return View(eNG_BOMDETAIL);
        }

        // GET: Admin/BomDetail/Edit/5
        public async Task<ActionResult> Edit(String PN, String VER, Decimal id)
        {
            if (String.IsNullOrEmpty(PN) || String.IsNullOrEmpty(VER))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Title = String.Format("物料{0}@{1}", PN, VER);
            ViewBag.SubTitle = "编辑";
            ViewBag.PN = PN;
            ViewBag.VER = VER;
            ViewBag.PATH = String.Format("/{0}.{1}/", PN, VER);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ENG_BOMDETAIL eNG_BOMDETAIL = await db.ENG_BOMDETAIL.FindAsync(id);
            if (eNG_BOMDETAIL == null)
            {
                return HttpNotFound();
            }
            await InitSelect(eNG_BOMDETAIL.SEMILINEGRP);
            return View(eNG_BOMDETAIL);
        }

        // POST: Admin/BomDetail/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(String PN, String VER, [Bind(Include = "ITEMID,PARTNO,PARTVER,L_OPNO,COMP_PARTNO,DESCRIPTION,UNITCONSUMEQTY,LotControl,SERIALCONTROL,IsKeyID,CNTCONTROL,ISSEMI,SEMILINEGRP")] ENG_BOMDETAIL eNG_BOMDETAIL)
        {
            if (String.IsNullOrEmpty(PN) || String.IsNullOrEmpty(VER))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Title = String.Format("物料{0}@{1}", PN, VER);
            ViewBag.SubTitle = "编辑";
            ViewBag.PN = PN;
            ViewBag.VER = VER;
            ViewBag.PATH = String.Format("/{0}.{1}/", PN, VER);

            if (ModelState.IsValid)
            {
                db.Entry(eNG_BOMDETAIL).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            await InitSelect(eNG_BOMDETAIL.SEMILINEGRP);
            return View(eNG_BOMDETAIL);
        }

        // GET: Admin/BomDetail/Delete/5
        public async Task<ActionResult> Delete(String PN, String VER, Decimal id)
        {
            if (String.IsNullOrEmpty(PN) || String.IsNullOrEmpty(VER))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Title = String.Format("物料{0}@{1}", PN, VER);
            ViewBag.SubTitle = "删除";
            ViewBag.PN = PN;
            ViewBag.VER = VER;
            ViewBag.PATH = String.Format("/{0}.{1}/", PN, VER);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ENG_BOMDETAIL eNG_BOMDETAIL = await db.ENG_BOMDETAIL.FindAsync(id);
            if (eNG_BOMDETAIL == null)
            {
                return HttpNotFound();
            }
            return View(eNG_BOMDETAIL);
        }

        // POST: Admin/BomDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(String PN, String VER, Decimal id)
        {
            if (String.IsNullOrEmpty(PN) || String.IsNullOrEmpty(VER))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Title = String.Format("物料{0}@{1}", PN, VER);
            ViewBag.SubTitle = "删除";
            ViewBag.PN = PN;
            ViewBag.VER = VER;
            ViewBag.PATH = String.Format("/{0}.{1}/", PN, VER);

            ENG_BOMDETAIL eNG_BOMDETAIL = await db.ENG_BOMDETAIL.FindAsync(id);
            db.ENG_BOMDETAIL.Remove(eNG_BOMDETAIL);
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
