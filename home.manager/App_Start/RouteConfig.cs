using System.Web.Mvc;
using System.Web.Routing;

namespace home.manager
{
    public class AppAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "App"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ViewTemplates", // Route name
                "manager/{*templatePath}", // URL with parameters
                new {controller = "Templates", action = "Render", templatePath = "templatePath"} // Parameter defaults
                );
        }
    }

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Default",
                "{controller}/{action}/{id}",
                new {controller = "Account", action = "Index", id = UrlParameter.Optional}
                );

            routes.MapRoute("Public",
                "{controller}/{action}/{id}",
                new { controller = "Public", action = "Note", id = UrlParameter.Optional }
                );
        }
    }
}