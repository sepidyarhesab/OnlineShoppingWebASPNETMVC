using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersOrders.Repository.Orders;


namespace WebApplicationOrders.Controllers
{
    public class TrackingController : Controller
    {
        // GET: Tracking
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public string MyOrders(string Code)
        {
            var result = RepOrders.MyOrders(Code);
            Session["TrackingOrder"] = result;
            Response.Redirect("Index");
            return Session["TrackingOrder"].ToString();
        }


    }
}