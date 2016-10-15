using System.Web.Optimization;

namespace home.manager
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/vendor/jquery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/vendor/jquery/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/vendor/jquery/jquery.unobtrusive*",
                "~/Scripts/vendor/jquery/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/vendor/angular/angular.js",
                "~/Scripts/vendor/angular/angular-ng-grid.js",
                "~/Scripts/vendor/ng-modules/ng-grid-flexible-height.js",
                "~/Scripts/vendor/angular-ui/ui-bootstrap.js",
                "~/Scripts/vendor/angular-ui/ui-bootstrap-tpls.js",
                "~/Scripts/vendor/angular/angular-resource.js",
                "~/Scripts/vendor/lodash.js",
                "~/Scripts/vendor/ng-modules/autocomplete.js",
                "~/Scripts/vendor/ng-modules/angular-file-upload.js",

                "~/Scripts/vendor/ng-modules/rangy-core.js",
                "~/Scripts/vendor/ng-modules/rangy-selectionsaverestore.js",

                "~/Scripts/vendor/ng-modules/textAngular.js",
                "~/Scripts/vendor/ng-modules/textAngular-sanitize.js",
                "~/Scripts/vendor/ng-modules/textAngularSetup.js",
                "~/Scripts/vendor/ng-modules/textAngular-rangy.min.js",
                
                "~/Scripts/vendor/Highcharts-4.0.1/js/highcharts.js",
                "~/Scripts/vendor/angular/angular-route.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/home/app.js",
                "~/Scripts/home/main.js",
                "~/Scripts/settings/settings.js",
                "~/Scripts/settings/settingsService.js",
                "~/Scripts/expenses/expenses.js",
                "~/Scripts/users/users.js",
                "~/Scripts/charts/charts.js",
                "~/Scripts/documents/documents.js",
                "~/Scripts/contacts/contacts.js",
                "~/Scripts/notes/notes.js",
                "~/Scripts/security/security.js",
                "~/Scripts/account/changePassword.js",
                "~/Scripts/users/userService.js",
                "~/Scripts/directives/navigation.js"
                ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-responsive.css"
                ));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/site.css",
          //      "~/Content/autocomplete.css",
                "~/Content/angular-file-upload.css",
                "~/Content/textAngular.css",
                "~/Content/font-awesome.css",
                "~/Content/ng-grid.css"
            ));

            bundles.Add(new StyleBundle("~/Content/ng-grid/css").Include(
               "~/Content/ng-grid/main.css"
            ));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                "~/Content/themes/base/jquery.ui.core.css",
                "~/Content/themes/base/jquery.ui.resizable.css",
                "~/Content/themes/base/jquery.ui.selectable.css",
                "~/Content/themes/base/jquery.ui.accordion.css",
                "~/Content/themes/base/jquery.ui.autocomplete.css",
                "~/Content/themes/base/jquery.ui.button.css",
                "~/Content/themes/base/jquery.ui.dialog.css",
                "~/Content/themes/base/jquery.ui.slider.css",
                "~/Content/themes/base/jquery.ui.tabs.css",
                "~/Content/themes/base/jquery.ui.datepicker.css",
                "~/Content/themes/base/jquery.ui.progressbar.css",
                "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new StyleBundle("~/Content/less/css").Include(
               "~/Content/less/main.css"
            ));
        }
    }
} 