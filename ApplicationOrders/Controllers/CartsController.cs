using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersGeneral.Repository.General;
using OrdersInventory.Repository.Inventory;
using OrdersOrders.Repository.Orders;
using OrdersOrders.ViewModels.Orders;

namespace ApplicationOrders.Controllers
{
    public class CartsController : Controller
    {
        // GET: Carts
        public ActionResult Index()
        {
            var type = RepSettings.RepositoryLoginTypeSelect();
            if (type == true)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var UserRef = Guid.Parse(User.Identity.Name);
                    if (Session["Carts"] != null)
                    {

                        var carts = Session["Carts"] as List<VMOrders.VmOrderSubmit>;
                        var resuult = RepOrders.RepositoryCarts(carts, UserRef);
                        return View(resuult);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Default");
                    }
                }
                else
                {
                    Session["UrlBack"] = "/Carts";
                    return RedirectToAction("Login", "Login");
                }
            }
            else
            {
                if (Session["Carts"] != null)
                {
                    var carts = Session["Carts"] as List<VMOrders.VmOrderSubmit>;
                    var resuult = RepOrders.RepositoryCarts(carts, Guid.Empty);
                    return View(resuult);
                }
                else
                {

                    return RedirectToAction("Index", "Default");
                }
            }
            //if (Session["Carts"] != null)
            //{
            //    var UserRef = Guid.Parse(User.Identity.Name);
            //    var carts = Session["Carts"] as List<VMOrders.VmOrderSubmit>;
            //    var resuult = RepOrders.RepositoryCarts(carts, UserRef);
            //    return View(resuult);
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Default");
            //}
        }


        [HttpPost]
        public ActionResult Index(string code)
        {
            if (Session["Carts"] != null)
            {
                var carts = Session["Carts"] as List<VMOrders.VmOrderSubmit>;
                var resuult = RepOrders.RepositoryCartsCode(carts, code);
                return View(resuult);
            }
            else
            {
                return RedirectToAction("Index", "Default");
            }
        }


        [HttpPost]
        public int UseDiscount(int sum, string code)
        {
            var rep = new RepProducts();
            var Discount = rep.UseDiscount(sum, code);
            return Discount;
        }
    }
}