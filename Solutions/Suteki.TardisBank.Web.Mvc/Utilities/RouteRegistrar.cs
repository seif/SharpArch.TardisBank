namespace Suteki.TardisBank.Web.Mvc.Utilities
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RouteRegistrar
    {
        public static void RegisterRoutesTo(RouteCollection routes) 
        { 
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }); // Parameter defaults

            routes.MapRoute(
                "WithRavenId", // Route name
                "{controller}/{action}/{entity}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", entity = UrlParameter.Optional, id = UrlParameter.Optional } // Parameter defaults
            );
        }
    }
}
