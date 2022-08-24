using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using OrdersGeneral.Repository.General;
using OrdersGeneral.ViewModels.General;
using SepidyarHesabExtensions.Classes;
using SepidyarHesabExtensions.Extentions;
using WebApplicationOrders.Models.Database.OrderDatabase;


namespace WebApplicationOrders.Controllers
{
    public class MenuManagementController : Controller
    {
        // GET: MenuManagement


        public ActionResult Index()
        {
            //if (User.Identity.IsAuthenticated)
            //{
            var query = RepMenu.RepositoryMainMenuManagement();
            return View(query);
            //}
            //return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult Index(string search)
        {
            if (search != "")
            {
                var result = RepMenu.RepositoryMainSearch(search);
                return View(result);
            }
            else
            {
                var query = RepMenu.RepositoryMainMenuManagement();
                return View(query);
            }
        }



        #region Add


        public ActionResult Add()
        {
            return View();
        }


        #endregion

        #region ChangeStatus

        public void ChangeStatus(Guid id)
        {
            var result = RepMenu.ChangeStatus(id);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/MenuManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/MenuManagement");
            }

        }


        #endregion


        #region Delete

        public void Delete(Guid id)
        {
            var result = RepMenu.DeleteRow(id);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/MenuManagement");
            }
            else if (result.Contains("TRUE"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/MenuManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/MenuManagement");
            }


        }



        #endregion

        #region Generate

        [HttpPost]

        public void Generate(VMMenu.VmMenuMainMenuManagement values)
        {
            
                if (values.PrimaryTitle != "")
                {
                    var Userid = Guid.Parse(User.Identity.Name);
                    var result = RepMenu.AddNewRow(values, Userid);
                    if (result.Contains("Error"))
                    {
                        TempData["JavaScriptFunction"] =
                            IziToast.Error("خطایی رخ داده است", "خطا در ایجاد سطر به دلیل : " + result);
                    }
                    else
                    {
                        TempData["JavaScriptFunction"] =
                            IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                    }
                }
                else
                {
                    TempData["JavaScriptFunction"] =
                        IziToast.Error("خطایی رخ داده است", "عنوان اصلی نمیتواند خالی باشد");
                }

                Response.Redirect("/MenuManagement/Index");
        }






        #endregion



        public ActionResult Edit(Guid id)
        {
            var result = RepMenu.RepositoryMainMenuManagementById(id);
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
        public void EditRow(VMMenu.VmMenuMainMenuManagement value)
        {
            var Userid = Guid.Parse(User.Identity.Name);
                var result = RepMenu.RepositoryMainMenuManagementEdit(value, Userid);
                if (result.Contains("Error"))
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                }
                else
                {
                    TempData["JavaScriptFunction"] =
                        IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                }

                Response.Redirect("/MenuManagement");
        }




        public ActionResult ListAccess(Guid id)
        {
            Session["UserAccessId"] = id;

            var result = RepMenuAdmin.RepAccessMangment(id);
            return View(result);
        }


        [HttpPost]
        public ActionResult AddMenuAccess(Guid menuRef, Guid userRef)
        {
            var db = new Orders_Entities();
            var query = db.Table_MenuAccess.ToList().Exists(c => c.UserRef == userRef && c.MenuRef == menuRef);
            if (!query)
            {
                var queryAdd = db.Table_MenuAccess.Add(new Table_MenuAccess
                {
                    Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_MenuAccess"),
                    UserRef = userRef,
                    MenuRef = menuRef,
                    IsDelete = false,
                    IsOk = false,
                    CreatorDate = DateTime.Now,
                    CreatorRef = userRef,
                    Id = Guid.NewGuid(),
                    ModifierRef = userRef,
                    ModifireDate = DateTime.Now,
                    Sort = 1,
                    Version = 1,
                });
                db.Table_MenuAccess.Add(queryAdd);
                db.SaveChanges();
            }

            if (Session["UserAccessId"] != null)
            {
                Response.Redirect("/MenuManagement/ListAccess/" + Session["UserAccessId"]);
            }

            return View();
        }



        public ActionResult MenuAccessChange(Guid id)
        {
            var db = new Orders_Entities();
            var query = db.Table_MenuAccess.FirstOrDefault(c => c.Id == id);
            if (query != null)
            {
                switch (query.IsOk)
                {
                    case true:
                    {
                        query.IsOk = false;
                        db.SaveChanges();
                        break;
                    }
                    case false:
                    {
                        query.IsOk = true;
                        db.SaveChanges();
                        break;
                    }
                }
            }

            if (Session["UserAccessId"] != null)
            {
                Response.Redirect("/MenuManagement/ListAccess/" + Session["UserAccessId"]);
            }

            return View("Index");
        }

        public ActionResult AdminAccess(Guid id)
        {
            Session["UserAccessId"] = id;
            var result = RepMenuAdmin.RepAdminAccessManagement(id);
            if (result != null)
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات با موفقیت امیز به پایان رسید", "دسترسی ها داده شد");
                Response.Redirect("/MenuManagement/ListAccess/" + Session["UserAccessId"]);
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/MenuManagement/ListAccess/" + Session["UserAccessId"]);
            }
            return View("Index");
        }
    }

}



