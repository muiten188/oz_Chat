using System.Web;
using System.Web.Optimization;

namespace CRMOZ.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                     "~/Content/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                     "~/Content/plugins/fastclick/lib/fastclick.js",
                     "~/Content/plugins/dist/js/adminlte.min.js",
                     "~/Content/plugins/dist/js/demo.js",
                     "~/Scripts/bootbox.js",
                     "~/Content/plugins/toastr/toastr.min.js",
                     "~/Content/plugins/toastr/custom-toastr.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/plugins/font-awesome/css/font-awesome.min.css",
                      "~/Content/plugins/Ionicons/css/ionicons.min.css",
                      "~/Content/plugins/dist/css/AdminLTE.min.css",
                      "~/Content/plugins/dist/css/skins/_all-skins.min.css",
                      "~/Content/plugins/toastr/toastr.min.css",
                      "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/lightgallery").Include(
                     "~/Content/plugins/lightGallery/dist/css/lightgallery.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/lightgallery").Include(
                    "~/Content/plugins/lightGallery/dist/js/lightgallery-all.min.js",
                     "~/Content/plugins/lightGallery/lib/jquery.mousewheel.min.js"));
        }
    }
}
