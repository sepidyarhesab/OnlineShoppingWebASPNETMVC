using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplicationHamtOrders
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Product",
                url: "Product/Id/{id}",
                defaults: new { controller = "Product", action = "Id", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Categories",
                url: "Categories/SubCategory/{id}",
                defaults: new { controller = "Categories", action = "SubCategory", id = UrlParameter.Optional }
            );


            routes.MapRoute(
                name: "ProductSelection",
                url: "ProductSelection/Product/{id}",
                defaults: new { controller = "ProductSelection", action = "Product", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "MainCategories",
                url: "Categories/MainCategory/{id}",
                defaults: new { controller = "Categories", action = "MainCategory", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "BlogSubCategories",
                url: "Blog/BlogSubCategory/{id}",
                defaults: new { controller = "Blog", action = "BlogSubCategory", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "BlogMainCategories",
                url: "Blog/BlogMainCategory/{id}",
                defaults: new { controller = "Blog", action = "BlogMainCategory", id = UrlParameter.Optional }
            );
        }
    }
}
