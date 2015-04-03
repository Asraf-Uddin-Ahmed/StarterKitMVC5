using System.Web;
using System.Web.Optimization;

namespace $safeprojectname$.App_Start
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));


            // Form Validator
            bundles.Add(new StyleBundle("~/Content/css/validator").Include(
                      "~/Content/validationEngine.jquery.css"));
            bundles.Add(new ScriptBundle("~/bundles/validator").Include(
                        "~/Scripts/jquery.validationEngine.js",
                        "~/Scripts/i18n/validationEngine/jquery.validationEngine-en.js"));

            // Custom Bundles
            bundles.Add(new StyleBundle("~/Content/css/custom").Include(
                      "~/Content/Custom.css"));
            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                        "~/Scripts/Custom/*.js"));

        }
    }
}
