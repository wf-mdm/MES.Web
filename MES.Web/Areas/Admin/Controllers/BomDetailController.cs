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
        private static List<SelectListItem> SERIALCONTROL_LIST = new List<SelectListItem>();
        static BomDetailController()
        {
            for (int i = 0; i < 9; i++)
            {
                String t = ((char)(i + (int)'1')).ToString();
                SERIALCONTROL_LIST.Add(new SelectListItem() { Text = t, Value = t });
            }
            for (int i = 0; i < 26; i++)
            {
                String t = ((char)(i + (int)'A')).ToString();
                SERIALCONTROL_LIST.Add(new SelectListItem() { Text = t, Value = t });
            }
        }

        private async Task InitSelect(String GRP, String SERIALCONTROL)
        {
            IList<SelectListItem> grps = await db.ENG_PRDLINE.Select(e => new SelectListItem()
            {
                Text = e.LINEGRP,
                Value = e.LINEGRP
            }).Distinct().ToListAsync();
            ViewBag.L_OPNO = await db.ENG_LINEOP.Select(e => new SelectListItem()
            {
                Text = e.L_OPNO,
                Value = e.L_OPNO
            }).Distinct().ToListAsync();
            ViewBag.SEMILINEGRP = new SelectList(grps, "Value", "Text", GRP);
            ViewBag.SERIALCONTROL = new SelectList(SERIALCONTROL_LIST, "Value", "Text", SERIALCONTROL);
        }

        // GET: Admin/BomDetail
        public async Task<ActionResult> Index(String LINENAME, String PN, String VER, ENG_BOMDETAIL Query)
        {
            if (String.IsNullOrEmpty(LINENAME) || String.IsNullOrEmpty(PN) || String.IsNullOrEmpty(VER))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Title = String.Format("产线:{0}, 物料{1}@{2}", LINENAME, PN, VER);
            ViewBag.SubTitle = "查询";
            ViewBag.Query = Query;
            ViewBag.PN = PN;
            ViewBag.VER = VER;
            ViewBag.PATH = String.Format("/{0}.{1}/", PN, VER);
            return View(await db.ENG_BOMDETAIL.Where(d =>
                d.LINENAME.Equals(LINENAME) && PN.Equals(d.PARTNO) && d.PARTVER.Equals(d.PARTVER)
            ).ToListAsync());
        }

        // GET: Admin/BomDetail/Create
        public async Task<ActionResult> Create(String LINENAME, String PN, String VER)
        {
            if (String.IsNullOrEmpty(LINENAME) || String.IsNullOrEmpty(PN) || String.IsNullOrEmpty(VER))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Title = String.Format("产线:{0}, 物料{1}@{2}", LINENAME, PN, VER);
            ViewBag.SubTitle = "新建";
            ViewBag.PN = PN;
            ViewBag.VER = VER;
            ViewBag.PATH = String.Format("/{0}.{1}/", PN, VER);

            ENG_BOMDETAIL eNG_BOMDETAIL = new ENG_BOMDETAIL()
            {
                LINENAME = LINENAME,
                PARTNO = PN,
                PARTVER = VER
            };
            await InitSelect("", "");
            return View(eNG_BOMDETAIL);
        }

        // POST: Admin/BomDetail/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(String LINENAME, String PN, String VER, [Bind(Include = "ITEMID,LINENAME,KEYINFO,PARTNO,PARTVER,L_OPNO,COMP_PARTNO,DESCRIPTION,UNITCONSUMEQTY,LotControl,SERIALCONTROL,IsKeyID,CNTCONTROL,ISSEMI,SEMILINEGRP")] ENG_BOMDETAIL eNG_BOMDETAIL)
        {
            if (String.IsNullOrEmpty(LINENAME) || String.IsNullOrEmpty(PN) || String.IsNullOrEmpty(VER))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Title = String.Format("产线:{0}, 物料{1}@{2}", LINENAME, PN, VER);
            ViewBag.SubTitle = "新建";
            ViewBag.LINENAME = LINENAME;
            ViewBag.PN = PN;
            ViewBag.VER = VER;
            ViewBag.PATH = String.Format("/{0}.{1}/", PN, VER);

            if (ModelState.IsValid)
            {
                eNG_BOMDETAIL.ITEMID = db.ENG_BOMDETAIL.Max(b => b.ITEMID) + 1;
                db.ENG_BOMDETAIL.Add(eNG_BOMDETAIL);
                await db.SaveChangesAsync();
                return RedirectToAction("/Index");
            }

            await InitSelect(eNG_BOMDETAIL.SEMILINEGRP, eNG_BOMDETAIL.SERIALCONTROL);
            return View(eNG_BOMDETAIL);
        }

        // GET: Admin/BomDetail/Edit/5
        public async Task<ActionResult> Edit(String LINENAME, String PN, String VER, Decimal id)
        {
            if (String.IsNullOrEmpty(LINENAME) || String.IsNullOrEmpty(PN) || String.IsNullOrEmpty(VER))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Title = String.Format("产线:{0}, 物料{1}@{2}", LINENAME, PN, VER);
            ViewBag.SubTitle = "编辑";
            ViewBag.LINENAME = LINENAME;
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
            await InitSelect(eNG_BOMDETAIL.SEMILINEGRP, eNG_BOMDETAIL.SERIALCONTROL);
            return View(eNG_BOMDETAIL);
        }

        // POST: Admin/BomDetail/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(String LINENAME, String PN, String VER, [Bind(Include = "ITEMID,LINENAME,KEYINFO,PARTNO,PARTVER,L_OPNO,COMP_PARTNO,DESCRIPTION,UNITCONSUMEQTY,LotControl,SERIALCONTROL,IsKeyID,CNTCONTROL,ISSEMI,SEMILINEGRP")] ENG_BOMDETAIL eNG_BOMDETAIL)
        {
            if (String.IsNullOrEmpty(LINENAME) || String.IsNullOrEmpty(PN) || String.IsNullOrEmpty(VER))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Title = String.Format("产线:{0}, 物料{1}@{2}", LINENAME, PN, VER);
            ViewBag.SubTitle = "编辑";
            ViewBag.LINENAME = LINENAME;
            ViewBag.PN = PN;
            ViewBag.VER = VER;
            ViewBag.PATH = String.Format("/{0}.{1}/", PN, VER);

            if (ModelState.IsValid)
            {
                db.Entry(eNG_BOMDETAIL).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("/Index");
            }
            await InitSelect(eNG_BOMDETAIL.SEMILINEGRP, eNG_BOMDETAIL.SERIALCONTROL);
            return View(eNG_BOMDETAIL);
        }

        // GET: Admin/BomDetail/Delete/5
        public async Task<ActionResult> Delete(String LINENAME, String PN, String VER, Decimal id)
        {
            if (String.IsNullOrEmpty(LINENAME) || String.IsNullOrEmpty(PN) || String.IsNullOrEmpty(VER))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Title = String.Format("产线:{0}, 物料{1}@{2}", LINENAME, PN, VER);
            ViewBag.SubTitle = "删除";
            ViewBag.LINENAME = LINENAME;
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
        public async Task<ActionResult> DeleteConfirmed(String LINENAME, String PN, String VER, Decimal id)
        {
            if (String.IsNullOrEmpty(LINENAME) || String.IsNullOrEmpty(PN) || String.IsNullOrEmpty(VER))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Title = String.Format("产线:{0}, 物料{1}@{2}", LINENAME, PN, VER);
            ViewBag.SubTitle = "删除";
            ViewBag.LINENAME = LINENAME;
            ViewBag.PN = PN;
            ViewBag.VER = VER;
            ViewBag.PATH = String.Format("/{0}.{1}/", PN, VER);

            ENG_BOMDETAIL eNG_BOMDETAIL = await db.ENG_BOMDETAIL.FindAsync(id);
            db.ENG_BOMDETAIL.Remove(eNG_BOMDETAIL);
            await db.SaveChangesAsync();
            return RedirectToAction("/Index");
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
