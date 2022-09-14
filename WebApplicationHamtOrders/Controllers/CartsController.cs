﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersExtentions.Extensions;
using OrdersGeneral.Repository.General;
using OrdersInventory.Repository.Inventory;
using OrdersOrders.Repository.Orders;
using OrdersOrders.ViewModels.Orders;
using SepidyarHesabExtensions.Extentions;


namespace WebApplicatioNewOrders.Controllers
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
            try
            {
                if (code != "")
                {
                    Session["CodeDis"] = code;
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
                                if (mes.Contains("DiscountMulti"))
                                {
                                    TempData["JavaScriptFunction"] = IziToast.Error("این کد تخفیف قابل استفاده نیست", "مشتری عزیز ؛ به دلیل داشتن تخفیف در خرید فعلی شما قادر به استفاده از تخفیف دیگری نیستید");
                                }
                                else
                                {
                                    TempData["JavaScriptFunction"] = IziToast.Error("کد تخفیف منقضی شده است یا به اتمام رسیده است.", "کد تخفیف منقضی شده است یا به اتمام رسیده است.");
                                }
                            }

                        }

                        return View(resuult);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Default");
                    }
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطای ورودی اطلاعات.", "کد تخفیف را وارد کنید");
                    return RedirectToAction("Index", "Carts");
                }
            }
            catch (Exception e)
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطای ورودی اطلاعات.", "کد تخفیف را وارد کنید");
                return RedirectToAction("Index", "Carts");
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