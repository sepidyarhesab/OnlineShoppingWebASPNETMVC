using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using OrdersExtentions.Extensions;
using Rozhano_WebApplication2.Extensions;
using SepidyarHesabExtensions.Classes;

namespace WebApplicationHamtOrders
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            var persianCulture = new PersianCulture();
            Thread.CurrentThread.CurrentCulture = persianCulture;
            Thread.CurrentThread.CurrentUICulture = persianCulture;
            OffensiveWordsList.GetOffensiveWorlds();
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            Application["Totaluser"] = 0;
            UsedSession.UsedAccess();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Application.Lock();
            Application["Totaluser"] = (int)Application["Totaluser"] + 1;
            Application.UnLock();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

            var persianCulture = new PersianCulture();
            Thread.CurrentThread.CurrentCulture = persianCulture;
            Thread.CurrentThread.CurrentUICulture = persianCulture;
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //Response.Redirect("/Default");
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}
