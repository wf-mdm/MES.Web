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
    public class BomsController : Controller
    {
        private static String ModelName = "BOM";
        private MESDbContext db = new MESDbContext();

        private async Task InitSelect(String LINENAME, String RT_NAME, String DEFAULT_CONFNAME)
        {
            IList<SelectListItem> routes = await db.ENG_ROUTE.Select(e => new SelectListItem()
            {
                Text = e.RT_NAME,
                Value = e.RT_NAME
            }).Distinct().ToListAsync();
            IList<SelectListItem> confs = await db.ENG_LINEOPPARAMCONF
                .Where(m => "PRD".Equals(m.PARAM_TYPE))
                .Select(e => new SelectListItem()
                {
                    Text = e.CONFNAME,
                    Value = e.CONFNAME
                }).Distinct().ToListAsync();
            IList<ENG_PRDLINE> lines1 = await db.ENG_PRDLINE.ToListAsync();
            IList<SelectListItem> lines = lines1.Select(l => {
                return new SelectListItem()
                {
                    Value = l.LINENAME,
                    Text = l.CodeName
                };
            }).ToList();
            lines.Add(new SelectListItem()
            {
                Value = "ALL",
                Text = "ALL"
            });

            ViewBag.LINENAME = new SelectList(lines, "Value", "Text", LINENAME);
            ViewBag.RT_NAME = new SelectList(routes, "Value", "Text", RT_NAME);
            ViewBag.DEFAULT_CONFNAME = new SelectList(confs, "Value", "Text", DEFAULT_CONFNAME);
        }

        // GET: Admin/Boms
        public async Task<ActionResult> Index(ENG_BOMHEADER Query)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "查询";
            ViewBag.Query = Query;
            IList<ENG_PRDLINE> lines1 = await db.ENG_PRDLINE.ToListAsync();
            IList<SelectListItem> lines = lines1.Select(l => {
                return new SelectListItem()
                {
                    Value = l.LINENAME,
                    Text = l.CodeName
                };
            }).ToList();
            lines.Insert(0, new SelectListItem()
            {
                Value = "ALL",
                Text = "ALL"
            });

            ViewBag.LINENAME = new SelectList(lines, "Value", "Text", Query.LINENAME);
            return View(await db.ENG_BOMHEADER
                .Where(d =>
                    (String.IsNullOrEmpty(Query.LINENAME)
                        || d.LINENAME.IndexOf(Query.LINENAME) > -1) &&
                    (String.IsNullOrEmpty(Query.PARTNO)
                        || d.PARTNO.IndexOf(Query.PARTNO) > -1))
                .ToListAsync());
        }

        // GET: Admin/Boms/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ENG_BOMHEADER eNG_BOMHEADER = await db.ENG_BOMHEADER.FindAsync(id);
            if (eNG_BOMHEADER == null)
            {
                return HttpNotFound();
            }
            return View(eNG_BOMHEADER);
        }

        // GET: Admin/Boms/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "新建";

            await InitSelect("", "", "");
            return View();
        }

        // POST: Admin/Boms/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "LINENAME,PARTNO,RT_NAME,DEFAULT_CONFNAME,DESCRIPTION")] ENG_BOMHEADER eNG_BOMHEADER)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "新建";
            if (ModelState.IsValid)
            {
                eNG_BOMHEADER.CREATETIME = DateTime.Now;
                eNG_BOMHEADER.UPDATETIME = DateTime.Now;
                db.ENG_BOMHEADER.Add(eNG_BOMHEADER);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            await InitSelect(eNG_BOMHEADER.LINENAME, eNG_BOMHEADER.RT_NAME, eNG_BOMHEADER.DEFAULT_CONFNAME);
            return View(eNG_BOMHEADER);
        }

        // GET: Admin/Boms/Edit/5
        public async Task<ActionResult> Edit(string LINENAME, string PARTNO, String PARTVER)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "编辑";
            ENG_BOMHEADER eNG_BOMHEADER = await db.ENG_BOMHEADER.FindAsync(LINENAME, PARTNO, PARTVER);
            if (eNG_BOMHEADER == null)
            {
                return HttpNotFound();
            }
            await InitSelect(eNG_BOMHEADER.LINENAME, eNG_BOMHEADER.RT_NAME, eNG_BOMHEADER.DEFAULT_CONFNAME);
            return View(eNG_BOMHEADER);
        }

        // POST: Admin/Boms/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "LINENAME,PARTNO,PARTVER,RT_NAME,DEFAULT_CONFNAME,DESCRIPTION")] ENG_BOMHEADER eNG_BOMHEADER)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "编辑";
            if (ModelState.IsValid)
            {
                eNG_BOMHEADER.UPDATETIME = DateTime.Now;
                db.Entry(eNG_BOMHEADER).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            await InitSelect(eNG_BOMHEADER.LINENAME, eNG_BOMHEADER.RT_NAME, eNG_BOMHEADER.DEFAULT_CONFNAME);
            return View(eNG_BOMHEADER);
        }

        // GET: Admin/Boms/Delete/5
        public async Task<ActionResult> Delete(String LINENAME, string PARTNO, String PARTVER)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "删除";
            ENG_BOMHEADER eNG_BOMHEADER = await db.ENG_BOMHEADER.FindAsync(LINENAME, PARTNO, PARTVER);
            if (eNG_BOMHEADER == null)
            {
                return HttpNotFound();
            }
            return View(eNG_BOMHEADER);
        }

        // POST: Admin/Boms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string LINENAME, string PARTNO, String PARTVER)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "删除";

            ENG_BOMHEADER eNG_BOMHEADER = await db.ENG_BOMHEADER.FindAsync(LINENAME, PARTNO, PARTVER);
            db.ENG_BOMHEADER.Remove(eNG_BOMHEADER);
            foreach (ENG_BOMDETAIL d in await db.ENG_BOMDETAIL.Where(b => (b.LINENAME.Equals(eNG_BOMHEADER.LINENAME) && b.PARTNO.Equals(eNG_BOMHEADER.PARTNO))).ToListAsync())
            {
                db.ENG_BOMDETAIL.Remove(d);
            }
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
