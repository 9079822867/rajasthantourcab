using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RajasthanTourCabN
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // ✅ ADMIN ROUTE FIRST (protect admin URLs)
            routes.MapRoute(
                name: "Admin",
                url: "Admin/{action}/{id}",
                defaults: new { controller = "Admin", action = "Dashboard", id = UrlParameter.Optional }
            );

            // ✅ ACCOUNT ROUTE (login) — must be before the slug route, otherwise
            //    "/Account" is captured by {slug} and 404s as a missing page
            routes.MapRoute(
                name: "Account",
                url: "Account/{action}/{id}",
                defaults: new { controller = "Account", action = "Index", id = UrlParameter.Optional }
            );

            // ✅ CABPRICE ROUTE — same reason as Account
            routes.MapRoute(
                name: "CabPrice",
                url: "CabPrice/{action}/{id}",
                defaults: new { controller = "CabPrice", action = "Dashboard", id = UrlParameter.Optional }
            );

            // ✅ SLUG ROUTE
            routes.MapRoute(
                name: "Slug",
                url: "{slug}",
                defaults: new { controller = "Home", action = "Page" }
            );

            // ✅ DEFAULT ROUTE LAST
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
