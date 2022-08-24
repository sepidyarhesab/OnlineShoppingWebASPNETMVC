using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersInventory.Repository.Inventory;
using OrdersInventory.ViewModels.Inventory;
using SepidyarHesabExtensions.Extentions;


namespace ApplicationOrders.Controllers
{
    public class ProductPropertyListController : Controller
    {
        // GET: ProductPropertyList
        public RepProducts rep = new RepProducts();

        public ActionResult Index(Guid id)
        {
            var result = rep.RepositoryMainProductListMangment(id);
            if (result != null)
            {
                Session["IdProductRef"] = id;
                return View(result);
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                return View("Index");
            }
        }
        [HttpPost]
        public void EditRow(VMProduct.ViewModelProductPropertyList value)
        {
         
            if (ModelState.IsValid)
            {
                var Userid = Guid.Parse(User.Identity.Name);
                var result = rep.RepositoryMainProductListManagemenetEditById(value, Userid);
                if (result.Contains("Error"))
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                }
            }
            Response.Redirect("/ProductManagment");
        }
        public ActionResult Edit(Guid id)
        {

            
                var result = rep.RepositoryMainProductListMangmentByid(id);
                if (result != null)
                {
                    return View(result);
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                    return View("Index");
                }
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public void Generate(VMProduct.ViewModelProductPropertyList values)
        {
            if (Session["IdProductRef"] != null)
            {
                var Userid = Guid.Parse(User.Identity.Name);
                var id = Guid.Parse(Session["IdProductRef"].ToString());
                var result = rep.AddNewProductListRow(values, id,Userid);
                if (result.Contains("Error"))
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "خطا در ایجاد سطر به دلیل : " + result);
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                }


                Response.Redirect("/ProductPropertyList/Index/" + id);
            }

        }
        public void Delete(Guid Id)
        {
            var Result = rep.DeletePropertyList(Id);
                var id = Guid.Parse(Session["IdProductRef"].ToString());

                if (Result.Contains("success"))
            {

                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/ProductPropertyList/Index/" + id);


            }

            if (Result.Contains("true"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/ProductPropertyList/Index/" + id);
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/ProductPropertyList/Index/" + id);
            }
        }
    }
}