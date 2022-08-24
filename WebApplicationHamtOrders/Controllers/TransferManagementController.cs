using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersGeneral.Repository.General;
using OrdersGeneral.ViewModels.General;
using OrdersOrders.Repository.Orders;
using OrdersOrders.ViewModels.Orders;
using SepidyarHesabExtensions.Extentions;

namespace WebApplicationHamtOrders.Controllers
{
    public class TransferManagementController : Controller
    {
        // GET: TransferManagement
        public ActionResult Index()
        {
            var result = RepTransfer.RepositoryMainTransferManagemenet();
            return View(result);
        }


        public ActionResult Edit(Guid id)
        {
            var result = RepTransfer.Edit(id);
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
        [HttpPost]
        public void EditRow(VMTransfer.VMTransferManagement value)
        {

            if (ModelState.IsValid)
            {
                var Userid = Guid.Parse(User.Identity.Name);
                var result = RepTransfer.RepositoryManagementEdit(value, Userid);
                if (result.Contains("Error"))
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                }
            }
            Response.Redirect("/TransferManagement/Index");


        }
        public ActionResult Create()
        {
            return View();
        }
        public void Delete(Guid Id)
        {

            var Result = RepTransfer.Delete(Id);


            if (Result.Contains("success"))
            {

                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/TransferManagement/Index");


            }

            if (Result.Contains("true"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/TransferManagement/Index");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/TransferManagement/Index");

            }

        }
        public void ChangeStatus(Guid id)
        {
            var Result = RepTransfer.ChangeStatus(id);


            if (Result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/TransferManagement/Index");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/TransferManagement/Index");
            }

        }
        [HttpPost]
        public void Generate(VMTransfer.VMTransferManagement values)
        {


                var Userid = Guid.Parse(User.Identity.Name);
                var result = RepTransfer.AddNew(values, Userid);
                if (result.Contains("Error"))
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "خطا در ایجاد سطر به دلیل : " + result);
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                }

                Response.Redirect("/TransferManagement/Index");
        }



    }
}