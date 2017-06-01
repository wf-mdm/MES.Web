using System.Web;
using System.Web.Optimization;

namespace MES.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/assets/css/bootstrap.css",
                "~/assets/css/font-awesome.css",
                "~/assets/css/AdminLTE.css",
                "~/assets/css/skins/_all-skins.css",
                "~/assets/plugins/iCheck/square/blue.css",
                "~/assets/css/app.css"
                ));
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/assets/plugins/jQuery/jquery-2.2.3.min.js",
                "~/assets/js/bootstrap.js",
                "~/assets/js/jquery.validate*",
                "~/assets/plugins/iCheck/icheck.js",
                "~/assets/plugins/slimScroll/jquery.slimscroll.min.js",
                "~/assets/plugins/fastclick/fastclick.js",
                "~/assets/js/routie.js",
                "~/assets/js/app.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/js/line").Include(
                "~/assets/js/handlebars.js",
                "~/assets/app/line/*.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/js/op").Include(
                "~/assets/js/handlebars.js",
                "~/assets/app/op/*.js"
                ));
        }
    }
}
