using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersGeneral.Repository.General;
using OrdersInventory.Repository.Inventory;
using OrdersOrders.Repository.Orders;
using OrdersOrders.ViewModels.Orders;
using SepidyarHesabExtensions.Extentions;


namespace WebApplicationOrders.Controllers
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
        }


        [HttpPost]
        public ActionResult Index(string code)
        {
            if (Session["Carts"] != null)
            {
                var carts = Session["Carts"] as List<VMOrders.VmOrderSubmit>;
                var resuult = RepOrders.RepositoryCartsCode(carts, code);
                //Session["Carts"] = null;
                Session["Carts"] = resuult.CartsItems;
                if (resuult.CartsItems.Count > 0)
                {
                    var mes = resuult.CartsItems.First().Message;
                    if (mes.Contains("Success"))
                    {
                        TempData["JavaScriptFunction"] = IziToast.Success("کد تخفیف با موفقیت اعمال شد.", "کد تخفیف با موفقیت اعمال شد.");
                    }
                    else
                    {
                        TempData["JavaScriptFunction"] = IziToast.Error("Error : " + resuult.CartsItems.First().Message, "کد تخفیف دارای خطا میباشد");
                    }
                    
                }
                
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