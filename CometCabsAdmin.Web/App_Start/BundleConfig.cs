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

            bundles.Add(new StyleBundle("~/bundles/bootstrap-css")
                .Include("~/Content/bootstrap.css")
                .Include("~/Content/bootstrap-theme.css")
                );

            bundles.Add(new StyleBundle("~/bundles/jquery-ui")
                .IncludeDirectory("~/Content/themes/base", "*.css")
                );

            bundles.Add(new StyleBundle("~/bundles/ngDialog-css")
                .Include("~/Content/ngDialog-custom-width.css")
                .Include("~/Content/ngDialog-theme-default.css")
                .Include("~/Content/ngDialog-theme-plain.css")
                .Include("~/Content/ngDialog.css")
                );

            bundles.Add(new ScriptBundle("~/bundles/jquery")
                .Include("~/Scripts/jQuery/jquery-1.11.2.js")
                .Include("~/Scripts/jQuery-UI/jquery-ui-1.11.3.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/angularjs")
                .Include("~/Scripts/AngularJS/angular.js")
                .Include("~/Scripts/AngularJS/angular-route.js")
                .Include("~/Scripts/AngularJS/ngDialog.js")
                // .IncludeDirectory("~/Scripts/AngularJS", "*.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-js")
                .Include("~/Scripts/Bootstrap/bootstrap.js")
                );
        }
    }
}