using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersGeneral.Repository.General;
using SepidyarHesabExtensions.Extentions;


namespace WebApplicationHamtOrders.Controllers
{
    public class CommentsManagementController : Controller
    {
        // GET: CommentsManagement
        public ActionResult Index()
        {
            var query = RepComments.RepositoryCommentsManagement();
            return View(query);
        }


        [HttpPost]
        public ActionResult Index(string search)
        {
            if (search != "")
            {
                var result = RepComments.RepositoryCommentsManagementSearch(search);
                return View(result);
            }
            else
            {
                var query = RepComments.RepositoryCommentsManagement();
                return View(query);
            }
        }

        public void Delete(Guid id)
        {
            var result = RepComments.Delete(id);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/CommentsManagement");
            }
            else if (result.Contains("TRUE"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/CommentsManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/CommentsManagement");
            }


        }

        public void ChangeStatus(Guid id)
        {
            var result = RepComments.ChangeStatus(id);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/CommentsManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/CommentsManagement");
            }
        }


        
    }
}