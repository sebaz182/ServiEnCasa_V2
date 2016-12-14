using System.Web.Optimization;

namespace SeguridadWebv2
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
            /*Javascript*/
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                     "~/Scripts/jquery.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/moment.js",
                      "~/Scripts/bootstrap-datepicker.min.js",
                      "~/Scripts/bootstrap-datetimepicker.min.js",
                      "~/Scripts/moment-with-locales.js",
                      "~/Scripts/mainServienCasaJs.js"));

            bundles.Add(new ScriptBundle("~/bundles/typeahead").Include(
                      "~/Scripts/typeahead.bundle.js",
                      "~/Scripts/handlebars.js"));

            bundles.Add(new ScriptBundle("~/bundles/adminPanelJs").Include(
                     "~/Scripts/bootstrap.js",
                     "~/Scripts/jquery.dcjqaccordion.2.7.js",
                     "~/Scripts/SeguridadDashboard.js"));

            /*Estilos*/
            bundles.Add(new StyleBundle("~/Content/css").Include(
                     "~/Content/font-awesome/css/font-awesome.css",
                      "~/Content/bootstrap.css",
                      "~/Content/typeaheadjs.css",
                      "~/Content/bootstrap-datetimepicker.css",
                      "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/cssdash").Include(
                     "~/Content/font-awesome/css/font-awesome.css",
                    "~/Content/SeguridadStyle.css",
                     "~/Content/bootstrap.css",
                     "~/Content/typeaheadjs.css",
                     "~/Content/bootstrap-datetimepicker.css",
                     "~/Content/Site.css"));
            

            bundles.Add(new StyleBundle("~/Content/adminPanel").Include(
                 "~/Content/font-awesome/css/font-awesome.css",
                 "~/Content/bootstrap.css",
                  "~/Content/SeguridadDashboard.css",
                   "~/Content/SeguridadResponsiveDash.css"
                ));
        }
    }
}