using System.Web;
using System.Web.Optimization;

namespace ParentAgent
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/datatables/jquery.datatables.js",
                        "~/Scripts/datatables/datatables.bootstrap.js",
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/allJS").Include(
                        "~/Content/ZoneTemplate/vendor/jquery/jquery.min.js",
                        "~/Content/ZoneTemplate/vendor/bootstrap/js/bootstrap.bundle.min.js",
                        "~/Content/ZoneTemplate/js/imagesloaded.pkgd.min.js",
                        "~/Content/ZoneTemplate/js/isotope.pkgd.min.js",
                        "~/Content/ZoneTemplate/js/filter.js",
                        "~/Content/ZoneTemplate/js/jquery.appear.js",
                        "~/Content/ZoneTemplate/js/owl.carousel.min.js",
                        "~/Content/ZoneTemplate/js/jquery.fancybox.min.js",
                        "~/Content/ZoneTemplate/js/script.js",
                        "~/Content/ZoneTemplate/js/contact_me.js",
                        "~/Content/ZoneTemplate/js/filter.js",
                        "~/Content/ZoneTemplate/js/imagesloaded.pkgd.min.js",
                        "~/Content/ZoneTemplate/js/isotope.pkgd.min.js",
                        "~/Content/ZoneTemplate/js/jqBootstrapValidation.js",
                        "~/Content/ZoneTemplate/js/jquery.appear.js",
                        "~/Content/ZoneTemplate/js/jquery.fancybox.min.js",
                        "~/Content/ZoneTemplate/js/owl.carousel.min.js",
                        "~/Content/ZoneTemplate/js/script.js",
                        "~/Scripts/toastr.js",
                        "~/Scripts/bootbox.js",
                        //"~/Scripts/bootstrap.js"
                        "~/Scripts/gsap.min.js",
                        "~/Scripts/hoverButtonAnimation.js",
                        "~/Scripts/chart.js",
                        "~/Scripts/jquery.lazy-master/jquery.lazy.min.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/ZoneTemplate/vendor/bootstrap/css/bootstrap.min.css",
                        "~/Content/ZoneTemplate/css/all.css",
                        "~/Content/ZoneTemplate/css/jquery.fancybox.min.css",
                        "~/Content/ZoneTemplate/css/owl.carousel.min.css",
                        "~/Content/ZoneTemplate/css/style.css",
                        "~/Content/toastr.css",
                        "~/Content/datatables/css/datatables.bootstrap.css"
                       //"~/Content/bootstrap.css",
                      //"~/Content/site.css"
                      ));
        }
    }
}
