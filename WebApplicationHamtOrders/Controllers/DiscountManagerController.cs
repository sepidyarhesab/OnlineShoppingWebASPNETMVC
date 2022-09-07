using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersInventory.Repository.Inventory;
using OrdersInventory.ViewModels.Inventory;
using OrdersOrders.Repository.Orders;
using OrdersOrders.ViewModels.Orders;
using SepidyarHesabExtensions.Extentions;


namespace WebApplicationHamtOrders.Controllers
{
    public class DiscountManagerController : Controller
    {
        // GET: DiscountManager


        public RepProducts rep = new RepProducts();


        #region Main
        //public ActionResult CodeIndex()
        //{

        //    if (Request.QueryString["serach"] != null)
        //    {
        //        var search = Request.QueryString["serach"];
        //        var result = RepDiscount.RepositoryDiscountManagement(search);
        //        var sort = result.OrderBy(c => c.Sort);
        //        if (result.Count > 0)
        //        {
        //            ModelState.Clear();
        //            return View(sort);
        //        }
        //        else
        //        {
        //            var resultt = RepDiscount.RepositoryDiscountManagement();
        //            var sort2 = resultt.OrderBy(c => c.IsOk);
        //            return View(sort2);
        //        }
        //    }

        //    else
        //    {
        //        var resultt = RepDiscount.RepositoryDiscountManagement();
        //        var sort2 = resultt.OrderBy(c => c.IsOk);
        //        return View(sort2);
        //    }
        //}


        [HttpPost]
        public ActionResult Discount(Guid Id, int Discount, string DiscountCode)
        {
            var Userid = Guid.Parse(User.Identity.Name);
            var result = RepDiscount.Discount(Id, Userid, Discount, DiscountCode);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
            }
            return RedirectToAction("CodeIndex", "DiscountManager");
        }



        #endregion



        #region Code
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var result = RepDiscount.RepositoryDiscountManagements();
                return View(result);
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                return View("CodeIndex");
            }
            return RedirectToAction("Index", "Login");
        }


        [HttpPost]
        public ActionResult AllDiscounts(int? Discount,int? DiscountFee, string DiscountCode,Guid DiscountUser, int discountquantity, string StartDate1, string EndDate1)
        {
            var Userid = Guid.Parse(User.Identity.Name);
            if (RepDiscount.AllDiscount(Discount,DiscountFee, DiscountCode, DiscountUser, StartDate1, EndDate1, discountquantity, Userid) != null)
            {
                this.TempData["JavaScriptFunction"] = (object)IziToast.Success("عملیات با موفقیت امیز به پایان رسید", "دسترسی ها داده شد");
                this.Response.Redirect("/DiscountManager/CodeIndex/");
            }
            else
            {
                this.TempData["JavaScriptFunction"] = (object)IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                this.Response.Redirect("/DiscountManager/CodeIndex/");
            }
            return (ActionResult)this.View("CodeIndex");
        }
        public void AllDelete(string Entities)
        {

            var Result = RepDiscount.AllDelete(Entities);


            if (Result.Contains("success"))
            {

                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/DiscountManager/CodeIndex");


            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/DiscountManager/CodeIndex");

            }

        }

        public ActionResult CodeIndex()
        {
            var resultt = RepDiscount.RepositoryDiscountManagement();
            if (resultt.Count > 0)
            {
                return View(resultt);
            }
            else
            {
                return View();
            }
        }

        public ActionResult CodeEdit(Guid id)
        {
            if (User.Identity.IsAuthenticated)
            {

                var result = RepDiscount.EditDiscount(id);
                if (result != null)
                {
                    return View(result);
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                    return View("CodeIndex");
                }
            }
            return RedirectToAction("Index", "Login");

        }
        [HttpPost]
        public void CodeEditRow(VMDiscount.VmDiscountManagement value)
        {

            if (ModelState.IsValid)
            {
                var Userid = Guid.Parse(User.Identity.Name);
                var result = RepDiscount.RepositoryDiscountManagementEdit(value, Userid);
                if (result.Contains("Error"))
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                }
            }
            Response.Redirect("/DiscountManager/Index");


        }
        public ActionResult CodeCreate()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                return View("CodeIndex");
            }
            return RedirectToAction("Index", "Login");

        }
        public void CodeDelete(Guid Id)
        {

            var Result = RepDiscount.DeleteDiscount(Id);


            if (Result.Contains("success"))
            {

                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/DiscountManager/Index");


            }

            if (Result.Contains("true"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/DiscountManager/Index");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/DiscountManager/Index");

            }

        }
        public void CodeChangeStatus(Guid id)
        {
            var Result = RepDiscount.ChangeDiscountStatus(id);


            if (Result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/DiscountManager/Index");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/DiscountManager/Index");
            }

        }
        [HttpPost]
        public void CodeGenerate(VMDiscount.VmDiscountManagement values)
        {


            if (values.DiscountCode != "")
            {
                var Userid = Guid.Parse(User.Identity.Name);
                var result = RepDiscount.AddNewDiscount(values, Userid);
                if (result.Contains("Error"))
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "خطا در ایجاد سطر به دلیل : " + result);
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                }
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "عنوان اصلی نمیتواند خالی باشد");
            }

            Response.Redirect("/DiscountManager/Index");
        }
        #endregion

        #region Product
        public ActionResult ProductIndex()
        {
            return View();
        }
        public ActionResult ProductAdd()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Index", "Login");

        }


        [HttpPost]
        public void ProductGenerate(VMProduct.ViewModelProductDiscount values)
        {

            if (values.PrimaryTitle != "")
            {
                var Userid = Guid.Parse(User.Identity.Name);
                var result = rep.AddNewProductDiscount(values, Userid);
                if (result.Contains("Error"))
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "خطا در ایجاد سطر به دلیل : " + result);
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                }
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "عنوان اصلی نمیتواند خالی باشد");
            }

            Response.Redirect("/DiscountManager/ProductIndex");
        }
        public void ProductChangeStatus(Guid Id)
        {
            var Result = rep.ChangeProductDiscountStatus(Id);


            if (Result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/DiscountManager/ProductIndex");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/DiscountManager/ProductIndex");
            }

        }
        public void ProductDelete(Guid Id)
        {
            var Result = rep.DeleteProductDiscount(Id);

            if (Result.Contains("success"))
            {

                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/DiscountManager/ProductIndex");


            }

            if (Result.Contains("true"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/DiscountManager/ProductIndex");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/DiscountManager/ProductIndex");
            }



        }
        //public ActionResult ProductEdit(Guid id)
        //{
        //    var result = rep.RepositoryProductDiscountMangmentByid(id);

        //    if (result != null)
        //    {
        //        return View(result);
        //    }
        //    else
        //    {
        //        TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
        //        return View("Index");
        //    }
        //}

        [HttpPost]
        //public void ProductEditRow(VMOrders.ViewModelProductDiscount value)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var Userid = Guid.Parse(User.Identity.Name);
        //        var result = rep.RepositoryProductDiscountEditById(value, Userid);

        //        if (result.Contains("Error"))
        //        {
        //            TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
        //        }
        //        else
        //        {
        //            TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
        //        }
        //    }
        //    Response.Redirect("/DiscountManager/ProductIndex");
        //}

        public ActionResult ProductSelection(Guid id)
        {
            Session["IdSelectionConfig"] = id;
            var query = rep.RepositoryMainProductsMangmentByDiscountRef(id);

            return View(query);
        }

        [HttpPost]
        public ActionResult ProductSelection(Guid id, string SearchKey)
        {
            Session["IdSelectionConfig"] = id;
            ViewBag.Id = id;
            ViewBag.SearchKey = SearchKey;
            var query = rep.RepositoryMainProductsMangmentByProductRefAndSearchKey(id, SearchKey);
            return View(query);
        }
        //public ActionResult AddSelectionProduct(Guid id)
        //{
        //    if (Session["IdSelectionConfig"] != null)
        //    {
        //        var DiscountRef = Guid.Parse(Session["IdSelectionConfig"].ToString());
        //        var query = rep.RepositoryMainProductAddToDiscountProductSelection(DiscountRef, id);
        //        if (query == "Success")
        //        {
        //            return Redirect("/DiscountManager/ProductSelection/" + DiscountRef);
        //        }
        //        else
        //        {
        //            return Redirect("/DiscountManager/ProductSelection/" + DiscountRef);
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index");
        //    }
        //}



        //public ActionResult SelectAll()
        //{
        //    var Discountid = Guid.Parse(Session["IdSelectionConfig"].ToString());
        //    if (Session["IdSelectionConfig"] != null)
        //    {

        //        var query = rep.RepositoryMainProductAddALLProductSelection(Discountid);
        //        if (query == "Success")
        //        {
        //            return Redirect("/DiscountManager/ProductSelection/" + Discountid);
        //        }
        //        else
        //        {
        //            return Redirect("/DiscountManager/ProductSelection/" + Discountid);
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index");
        //    }
        //}

        //public ActionResult DeSelectAll()
        //{

        //    if (Session["IdSelectionConfig"] != null)
        //    {
        //        var DiscountRef = Guid.Parse(Session["IdSelectionConfig"].ToString());
        //        var query = rep.RepositoryMainProductRemoveALLProductSelection(DiscountRef);
        //        if (query == "Success")
        //        {
        //            return Redirect("/DiscountManager/ProductSelection/" + DiscountRef);
        //        }
        //        else
        //        {
        //            return Redirect("/DiscountManager/ProductSelection/" + DiscountRef);
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index");
        //    }
        //}
        #endregion


    }
}