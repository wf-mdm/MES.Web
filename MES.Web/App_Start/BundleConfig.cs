using System.Web;
using System.Web.Optimization;

namespace MES.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/assets/css/all").Include(
                "~/assets/css/bootstrap.css",
                "~/assets/css/font-awesome.css",
                "~/assets/css/AdminLTE.css",
                "~/assets/css/skins/_all-skins.css",
                "~/assets/plugins/iCheck/square/blue.css",
                "~/assets/plugins/bootstrap-datetimepicker/css/bootstrap-datetimepicker.css",
                "~/assets/plugins/select2/select2.min.css",
                "~/assets/css/app.css"
                ));
            bundles.Add(new StyleBundle("~/assets/css/layout").Include(
                "~/assets/css/layout.css"
                ));
            bundles.Add(new ScriptBundle("~/assets/js/base").Include(
                "~/assets/plugins/jQuery/jquery-2.2.3.min.js",
                "~/assets/plugins/jQueryUI/jquery-ui.min.js",
                "~/assets/js/bootstrap.js",
                "~/assets/js/jquery.validate.js",
                "~/assets/plugins/iCheck/icheck.js",
                "~/assets/plugins/slimScroll/jquery.slimscroll.min.js",
                "~/assets/plugins/fastclick/fastclick.js",
                "~/assets/plugins/select2/select2.min.js",
                "~/assets/plugins/bootstrap-datetimepicker/js/bootstrap-datetimepicker.js",
                "~/assets/plugins/bootstrap-datetimepicker/js/locales/bootstrap-datetimepicker.zh-CN.js",
                "~/assets/js/routie.js",
                "~/assets/js/app.js"
                ));
            bundles.Add(new ScriptBundle("~/assets/js/admin").Include(
                "~/assets/js/handlebars.js",
                "~/assets/admin/*.js"
                ));
            bundles.Add(new ScriptBundle("~/assets/js/common").Include(
                "~/assets/js/handlebars.js",
                "~/assets/app/common/FormUtils.js"
                ));
            bundles.Add(new ScriptBundle("~/assets/js/workshop").Include(
                "~/assets/app/workshop/*.js"
                ));
            bundles.Add(new ScriptBundle("~/assets/js/layout").Include(
                "~/assets/layout/data.js"
                ));

            bundles.Add(new ScriptBundle("~/assets/js/line").Include(
                "~/assets/app/line/*.js"
                ));
            bundles.Add(new ScriptBundle("~/assets/js/stn").Include(
                "~/assets/app/stn/*.js"
                ));
            bundles.Add(new ScriptBundle("~/assets/js/wh").Include(
                "~/assets/app/wh/*.js"
                ));
            bundles.Add(new ScriptBundle("~/assets/js/rpt").Include(
                "~/assets/js/handlebars.js",
                "~/assets/rpt/*.js"
                ));
        }
    }
}
