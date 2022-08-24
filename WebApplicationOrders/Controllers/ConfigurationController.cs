using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersInventory.Repository.Inventory;
using PagedList;


namespace WebApplicationOrders.Controllers
{
    public class ConfigurationController : Controller
    {
        // GET: Configuration
        public RepProducts rep = new RepProducts();

        public ActionResult ShowAllConfiguration(Guid id, int? page)
        {

            var result = rep.RepositoryMainProductShowAllProductByConfigurationRef(id).ToList();

            if (result.Count > 0)
            {
                int pageSize = 12;
                int pageNumber = (page ?? 1);
                return View(result.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return RedirectToAction("Index", "Default");
            }

        }

    }
}