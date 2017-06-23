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
    [Authorize(Roles = "OprRoles")]
    public class OprRolesController : Controller
    {
        private static String ModelName = "用户角色";
        private MESDbContext db = new MESDbContext();

        // GET: Admin/OprRoles
        public async Task<ActionResult> Index(HR_ROLES Querys)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "查询";
            ViewBag.Query = Querys;
            var hR_ROLES = db.HR_ROLES.Include(h => h.User);
            return View(await hR_ROLES
                .Where(r =>
                    (String.IsNullOrEmpty(Querys.USERID) ||
                    r.USERID.IndexOf(Querys.USERID) > -1 ||
                    r.User.OPERNAME.IndexOf(Querys.USERID) > -1)
                    && (String.IsNullOrEmpty(Querys.ROLEID) || r.ROLEID == Querys.ROLEID)
                 )
                .ToListAsync());
        }

        // GET: Admin/OprRoles/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HR_ROLES hR_ROLES = await db.HR_ROLES.FindAsync(id);
            if (hR_ROLES == null)
            {
                return HttpNotFound();
            }
            return View(hR_ROLES);
        }

        // GET: Admin/OprRoles/Create
        public ActionResult Create()
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "新建";

            ViewBag.USERID = new SelectList(db.HR_OPERATORS, "OPERID", "Name");
            return View();
        }

        // POST: Admin/OprRoles/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "USERID,ROLEID")] HR_ROLES hR_ROLES)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "新建";
            if (ModelState.IsValid)
            {
                db.HR_ROLES.Add(hR_ROLES);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.USERID = new SelectList(db.HR_OPERATORS, "OPERID", "Name", hR_ROLES.USERID);
            return View(hR_ROLES);
        }

        // GET: Admin/OprRoles/Delete/5
        public async Task<ActionResult> Delete([Bind(Include = "USERID,ROLEID")] HR_ROLES hR_ROLES)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "删除";

            HR_ROLES role = await db.HR_ROLES
                .Include(r1 => r1.User)
                .Where(r => r.USERID.Equals(hR_ROLES.USERID) && r.ROLEID.Equals(hR_ROLES.ROLEID)).SingleOrDefaultAsync();
            return View(role);
        }

        // POST: Admin/OprRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed([Bind(Include = "USERID,ROLEID")] HR_ROLES hR_ROLES)
        {
            ViewBag.Title = ModelName;
            ViewBag.SubTitle = "删除";

            HR_ROLES role = await db.HR_ROLES
                .Where(r => r.USERID.Equals(hR_ROLES.USERID) && r.ROLEID.Equals(hR_ROLES.ROLEID)).SingleOrDefaultAsync();
            db.HR_ROLES.Remove(role);
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
