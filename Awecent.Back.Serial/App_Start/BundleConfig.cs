using System.Web;
using System.Web.Optimization;

namespace Awecent.Back.Serial
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-notify.min.js",
                      "~/Scripts/notify.js",
                      "~/Scripts/respond.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Spiner.css"));



            bundles.Add(new ScriptBundle("~/bundles/report").Include(
                      "~/Scripts/highcharts-custom.js",
                      "~/Scripts/moment.js",
                      "~/Scripts/bootstrap-datetimepicker.min.js",
                      "~/Scripts/bootstrap-datepicker.min.js",
                      "~/Scripts/chosen.jquery.min.js"));

            bundles.Add(new StyleBundle("~/Content/report").Include(
                      "~/Content/bootstrap-datepicker.min.css",
                      "~/Content/bootstrap-datetimepicker.min.css",
                      "~/Content/bootstrap-chosen.css"));



            bundles.Add(new ScriptBundle("~/bundles/amcharts").Include(
                      "~/Scripts/amcharts/amcharts.js",
                      "~/Scripts/amcharts/serial.js",
                      "~/Scripts/amcharts/themes/light.js",
                      "~/Scripts/amcharts/themes/dark.js",
                      "~/Scripts/amcharts/amstock.js",

                      "~/Scripts/amcharts/plugins/export/export.js",
                      "~/Scripts/amcharts/plugins/responsive/responsive.js"));

            bundles.Add(new StyleBundle("~/Content/amcharts").Include(
                      "~/Scripts/amcharts/style.css",
                      "~/Scripts/amcharts/plugins/export/export.css"
                        ));


        }
    }
}
