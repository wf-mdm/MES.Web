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
    public class OprCapsController : Controller
    {
		private static String ModelName = "操作权限";
        private MESDbContext db = new MESDbContext();
        private async Task InitSelect(String OPERID, String LINENAME, String OP = "", String STN = "")
        {
            ViewBag.LINENAME = new SelectList(await db.ENG_PRDLINE.ToListAsync(), "LINENAME", "CodeName", LINENAME);
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

            ViewBag.OPERID= new SelectList(await db.V_USERANDROLES.ToListAsync(), "OPERID", "Name", OPERID);
        }

        // GET: Admin/OprCaps
        public async Task<ActionResult> Index(HR_OPERCAPBMATRIX Query)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "查询";
            ViewBag.Query = Query;
            await InitSelect("", "");
            var hR_OPERCAPBMATRIX = db.HR_OPERCAPBMATRIX.Include(h => h.Oper)
                .Where(d=>
                    String.IsNullOrEmpty(Query.OPERID) || Query.OPERID.Equals(d.OPERID)
                    && String.IsNullOrEmpty(Query.LINENAME) || Query.OPERID.Equals(d.LINENAME)
                );
            return View(await hR_OPERCAPBMATRIX.ToListAsync());
        }

        // GET: Admin/OprCaps/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HR_OPERCAPBMATRIX hR_OPERCAPBMATRIX = await db.HR_OPERCAPBMATRIX.FindAsync(id);
            if (hR_OPERCAPBMATRIX == null)
            {
                return HttpNotFound();
            }
            return View(hR_OPERCAPBMATRIX);
        }

        // GET: Admin/OprCaps/Create
        public async Task<ActionResult> Create()
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "新建";

            await InitSelect("", "");
            return View();
        }

        // POST: Admin/OprCaps/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "OPERID,LINENAME,L_OPNO,L_STNO,CAPBLEVEL,COMMENTS")] HR_OPERCAPBMATRIX hR_OPERCAPBMATRIX)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "新建";
            if (ModelState.IsValid)
            {
                db.HR_OPERCAPBMATRIX.Add(hR_OPERCAPBMATRIX);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            await InitSelect(hR_OPERCAPBMATRIX.OPERID, hR_OPERCAPBMATRIX.LINENAME, hR_OPERCAPBMATRIX.L_OPNO, hR_OPERCAPBMATRIX.L_STNO);
            return View(hR_OPERCAPBMATRIX);
        }

        // GET: Admin/OprCaps/Edit/5
        public async Task<ActionResult> Edit(String OPERID, String LINENAME, String L_OPNO)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "编辑";
            HR_OPERCAPBMATRIX hR_OPERCAPBMATRIX = await db.HR_OPERCAPBMATRIX.FindAsync(OPERID, LINENAME, L_OPNO);
            if (hR_OPERCAPBMATRIX == null)
            {
                return HttpNotFound();
            }
            await InitSelect(hR_OPERCAPBMATRIX.OPERID, hR_OPERCAPBMATRIX.LINENAME, hR_OPERCAPBMATRIX.L_OPNO, hR_OPERCAPBMATRIX.L_STNO);
            return View(hR_OPERCAPBMATRIX);
        }

        // POST: Admin/OprCaps/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "OPERID,LINENAME,L_OPNO,L_STNO,CAPBLEVEL,COMMENTS")] HR_OPERCAPBMATRIX hR_OPERCAPBMATRIX)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "编辑";
            if (ModelState.IsValid)
            {
                db.Entry(hR_OPERCAPBMATRIX).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            await InitSelect(hR_OPERCAPBMATRIX.OPERID, hR_OPERCAPBMATRIX.LINENAME, hR_OPERCAPBMATRIX.L_OPNO, hR_OPERCAPBMATRIX.L_STNO);
            return View(hR_OPERCAPBMATRIX);
        }

        // GET: Admin/OprCaps/Delete/5
        public async Task<ActionResult> Delete(String OPERID, String LINENAME, String L_OPNO)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "删除";
            HR_OPERCAPBMATRIX hR_OPERCAPBMATRIX = await db.HR_OPERCAPBMATRIX.FindAsync(OPERID, LINENAME, L_OPNO);
            if (hR_OPERCAPBMATRIX == null)
            {
                return HttpNotFound();
            }
            return View(hR_OPERCAPBMATRIX);
        }

        // POST: Admin/OprCaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(String OPERID, String LINENAME, String L_OPNO)
        {
			ViewBag.Title = ModelName;
			ViewBag.SubTitle = "删除";

            HR_OPERCAPBMATRIX hR_OPERCAPBMATRIX = await db.HR_OPERCAPBMATRIX.FindAsync(OPERID, LINENAME, L_OPNO);
            db.HR_OPERCAPBMATRIX.Remove(hR_OPERCAPBMATRIX);
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
