using MES.Web.Identity;
using MES.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MES.Web.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private MESSignInManager _signInManager;
        private MESUserManager _userManager;

        public MESSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<MESSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public MESUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<MESUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private ActionResult RedirectToLocal(string returnUrl, LoginModel m)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", m.AppId);
        }

        private void InitApps(LoginModel model)
        {
            SelectList sl = new SelectList(new List<SelectListItem>()
            {
                new SelectListItem(){Text ="产线", Value = "Line"},
                new SelectListItem(){Text ="管理端", Value = "Admin"}
            },
            "Value", "Text", model == null ? "" : model.AppId);

            ViewBag.AppList = sl;
        }

        // GET: Session
        [AllowAnonymous]
        public ActionResult Index()
        {
            InitApps(null);
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(LoginModel model, string returnUrl)
        {
            InitApps(model);
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.UserId, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl, model);
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "用户名或者密码错误。");
                    return View(model);
            }
        }

        //
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Session");
        }

    }
}