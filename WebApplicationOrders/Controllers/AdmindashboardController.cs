using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersDatabase.Models;
using OrdersGeneral.Repository.General;



namespace WebApplicationOrders.Controllers
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



