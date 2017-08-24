using MES.Web.Identity;
using MES.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MES.Web.Controllers
{
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

        private ActionResult RedirectToLocal(LoginModel m)
        {
            if (Url.IsLocalUrl(m.ReturnUrl))
            {
                return Redirect(m.ReturnUrl);
            }
            return RedirectToAction("Index", m.AppId);
        }

        private void InitApps(LoginModel model)
        {
            SelectList sl = new SelectList(new List<SelectListItem>()
            {
                new SelectListItem(){Text ="产线", Value = "Line"},
                new SelectListItem(){Text ="WMS", Value = "Warehouse"},
                new SelectListItem(){Text ="管理端", Value = "Admin"}
            },
            "Value", "Text", model == null ? "" : model.AppId);

            ViewBag.AppList = sl;
        }

        static Regex DEFAULT_URL = new Regex("^/app/Admin/?$", RegexOptions.IgnoreCase);
        // GET: Session
        [AllowAnonymous]
        public ActionResult Index(LoginModel model)
        {
            if (!String.IsNullOrEmpty(model.ReturnUrl) && DEFAULT_URL.IsMatch(model.ReturnUrl))
                model.ReturnUrl = null;
            InitApps(null);
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginModel model)
        {
            InitApps(model);
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.UserId, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model);
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "用户名或者密码错误。");
                    return View("Index", model);
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