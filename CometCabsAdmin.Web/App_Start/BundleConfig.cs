using System.Web.Optimization;

namespace CometCabsAdmin.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/styles")
                .Include("~/Content/Site.css")
                );

            bundles.Add(new StyleBundle("~/bundles/bootstrap")
                .Include("~/Content/bootstrap.css")
                .Include("~/Content/bootstrap-theme.css")
                );

            bundles.Add(new StyleBundle("~/bundles/jqueryui")
                .IncludeDirectory("~/Content/themes/base", "*.css")
                );

            bundles.Add(new ScriptBundle("~/bundles/scripts")
                .Include("~/Scripts/jquery-1.9.1.js")
                .Include("~/Scripts/jquery-ui-1.11.3.js")
                .Include("~/Scripts/angular-1.3.14.js")
                .Include("~/Scripts/bootstrap.js")
                );
        }
    }
}