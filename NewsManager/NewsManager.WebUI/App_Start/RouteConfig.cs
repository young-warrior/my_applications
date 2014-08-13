using System.Web.Mvc;
using System.Web.Routing;

namespace NewsManager.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapRoute(null,
            "",
            new
        {
            controller = "News",
            action = "Index",
            category = (string)null,
            page = 1
        }
      );
        routes.MapRoute(null,
       "Page{page}",
        new { Controller = "News", action = "Index",caregory =(string)null },
        new {page = @"\d+"}
      );
        
            routes.MapRoute(null,
            "{category}",
            new { controller = "News", action = "Index", page = 1 }
          );

            routes.MapRoute(null,
              "{category}/Page{page}",
              new { controller = "News", action = "Index" },
              new { page = @"\d+" }
            );

            routes.MapRoute(null, "{controller}/{action}");
        }

    }
}