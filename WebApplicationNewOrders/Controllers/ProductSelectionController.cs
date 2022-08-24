using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersInventory.Repository.Inventory;
using PagedList;


namespace WebApplicationNewOrders.Controllers
{
    public class ProductSelectionController : Controller
    {
        // GET: ProductSelection
        public ActionResult Index()
        {
            return View();
        }
        public RepProducts rep = new RepProducts();

        public ActionResult Product(Guid id, int? page = 1)
        {
            var result = rep.RepositoryMainProductShowAllProductByConfigurationRef(id).ToList();
            if (result.Count > 0)
            {
                int pageSize = 16;
                int pageNumber = (page ?? 1);
                return View(result.ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Index", "Default");
        }
    }
}