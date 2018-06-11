using System.Web.Mvc;

namespace MES.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_BomDetail",
                "app/Admin/{controller}/{LINENAME}/{PN}/{VER}/{action}/{id}",
                new { action = "Index", controller = "BomDetail", id = UrlParameter.Optional },
                new { controller = "BomDetail" }
            );
            context.MapRoute(
                "Admin_ContnrAttrs",
                "app/Admin/{controller}/{CONTNRTYPE}/{action}/{id}",
                new { action = "Index", controller = "ContnrAttrs", id = UrlParameter.Optional },
                new { controller = "ContnrAttrs" }
            );
            context.MapRoute(
                "Admin_default",
                "app/Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}