using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Site
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",

                // /home/user/1
                // The "home" portion tells MVC to look for a controller called "HomeController.cs"
                // The "action" portion tells MVC to look for a method on that controller
                // Notice that the "id" portion is considered optional in the following "defaults" portion
                // The other values "Home" and "Index" are default values if those values are not provided
                // (Index is one of the methods in the home controller)
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
