using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplicationOrders.Controllers
{
    public class BlogCategoriesController : Controller
    {
        // GET: BlogCategories
        public ActionResult Index()
        {
            return View();
        }
    }
}