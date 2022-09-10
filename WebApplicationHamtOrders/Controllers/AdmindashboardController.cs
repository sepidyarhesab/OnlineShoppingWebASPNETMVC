using System;
using System.ComponentModel;
using System.Configuration;
using System.IO.Compression;
using System.Net;
using System.Web.Hosting;
using System.Web.Mvc;
using OrdersGeneral.Repository.General;
using OrdersDatabase.Models;


namespace WebApplicationHamtOrders.Controllers
{
    public class AdmindashboardController : Controller
    {
        // GET: Admindashboard

        public ActionResult Index()
        {
            var db = new Orders_Entities();
            if (User.Identity.IsAuthenticated)
            {
                var id = Guid.Parse(User.Identity.Name);
                var rep = RepUsers.GetRole(id);
                if (rep.Contains("Admin"))
                {
                    return View();
                    //var version = new VersionControl();
                    //var result = version.ChechVersion();
                    //if (result.IsOk)
                    //{
                    //    return View();
                    //}
                    //else
                    //{
                    //    string updateUrl = ConfigurationManager.AppSettings["UpdateUrl"];
                    //    if (result.ApplicationVersionDatabase == result.ApplicationVersionAssembly)
                    //    {
                    //        if (result.DatabaseVersionDatabase == result.DatabaseVersionAssembly)
                    //        {

                    //        }
                    //        else
                    //        {

                    //        }
                    //    }
                    //    else
                    //    {
                    //        WebClient webClient = new WebClient();
                    //        webClient.DownloadFile(updateUrl + result.ApplicationVersionAssembly + ".zip", HostingEnvironment.MapPath("~/" + result.ApplicationVersionAssembly + ".zip"));
                    //        string zipPath = System.Web.HttpContext.Current.Server.MapPath("/Update/" + result.ApplicationVersionAssembly + ".zip");
                    //        string extractPath = System.Web.HttpContext.Current.Server.MapPath("~/");
                    //        ZipFile.ExtractToDirectory(zipPath, extractPath);
                    //    }
                    //}
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            return RedirectToAction("Index", "Login");
        }




    }
}



