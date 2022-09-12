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
    public class UserManagementController : Controller
    {
        // GET: UserManagement
        public ActionResult Index()
        {
            Session["UserAccessId"] = null;
            var query = RepUsers.RepositoryUserManagment();
            return View(query);
        }

        [HttpPost]
        public ActionResult Index(string search)
        {
            Session["UserAccessId"] = null;
            if (search != "")
            {
                var result = RepUsers.RepositoryUserManagmentSearch(search);
                return View(result);
            }
            else
            {
                var query = RepUsers.RepositoryUserManagment();
                return View(query);
            }
        }

        public void ChangeStatus(Guid Id)
        {
            var Result = RepUsers.ChangeUserStatus(Id);


            if (Result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/UserManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/UserManagement");
            }

        }
        public void ChangeBlackList(Guid Id)
        {
            var Result = RepUsers.ChangeUserBlackList(Id);


            if (Result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/UserManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/UserManagement");
            }

        }

        public void Delete(Guid Id)
        {
            var Result = RepUsers.DeleteUser(Id);
            if (Result.Contains("Error"))
            {

                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
            }
            else if (Result.Contains("true"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/UserManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/UserManagement");
            }
        }

        public void AddToDeleteUser(Guid Id)
        {
            var result = RepUsers.AddToDeleteUser(Id);
            if (result.Contains("success"))
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/UserManagement");
            }

            else if (result.Contains("true"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/UserManagement");
            }

            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/UserManagement");
            }
        }


        public void UnDelete(Guid Id)
        {
            var Result = RepUsers.UnDeleteUser(Id);

            if (Result.Contains("success"))
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/UserManagement/DeletedUsers");
            }

            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/UserManagement/DeletedUsers");
            }
        }
        public ActionResult DeletedUsers()
        {
            return View();
        }

        public ActionResult Add()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Index", "Login");
        }
        [HttpPost]
        public void Generate(VMUser.VMUsers values)
        {
            if (values.Name != "")
            {
                if (values.Family != "")
                {
                    if (values.Phone != "")
                    {
                        if (values.Password != "")
                        {
                            var Userid = Guid.Parse(User.Identity.Name);
                            var result = RepUsers.AddNewUser(values, Userid);
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

                        }
                    }
                    else
                    {

                    }
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "عنوان اصلی نمیتواند خالی باشد");
                }

            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نام نمیتواند خالی باشد");
            }
            Response.Redirect("/UserManagement");
        }


        public ActionResult Edit(Guid id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var result = RepUsers.RepositoryUserMangmentByid(id);
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
            return RedirectToAction("Index", "Login");

        }
        [HttpPost]
        public void EditRow(VMUser.VMUsers value)
        {

            if (ModelState.IsValid)
            {
                var Userid = Guid.Parse(User.Identity.Name);
                var result = RepUsers.RepositoryUserMangmentEditById(value, Userid);
                if (result.Contains("Error"))
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                }
            }
            Response.Redirect("/UserManagement/Index");


        }
        //End--------------------------------------
        //Add new address for customers by Admin
        public ActionResult AddressAddAdmin(Guid id)
        {
            Session["id"] = id;
            return View();
        }

        [HttpPost]
        public void GenerateAddressAdmin(VMUser.VMUsers value)
        {
            var id = Session["id"].ToString();
            Guid id1 = Guid.Parse(id);
            var Userid = Guid.Parse(User.Identity.Name);
            if (User.Identity.IsAuthenticated)
            {
                if (Userid != null)
                {
                    var result = RepUsers.AddNewAddress(value, Userid, id1);
                    if (result.Contains("Success"))
                    {
                        TempData["JavaScriptFunction"] = IziToast.Success("عملیات با موفقیت انجام شد", "عملیات با موفقیت انجام شد");
                        if (Session["UserLocationRef"] != null)
                        {
                            Response.Redirect("/UserManagement/IndexAddressAdmin/" + Session["id"]);
                        }
                        else
                        {
                            Response.Redirect("/UserManagement/IndexAddressAdmin/" + Session["id"]);
                        }
                    }
                    else
                    {
                        TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                        Response.Redirect("/UserManagement/IndexAddressAdmin/" + Session["id"]);
                    }
                }
            }
           
        }
        //End-----------------------------------------
        //Index Address by Admin
        public ActionResult IndexAddressAdmin(Guid id)
        {
            Session["UserID"] = id;
            var query = RepUsers.RepListDashboardAddressAdmin(id);
            return View(query);
        }
        //End--------------------------------------------
        //Change IsMain For Address by Admin
        public void ChangeStatusIsMain(Guid Id)
        {
            var Result = RepUsers.ChangeIsMainUserAddressStatus(Id);


            if (Result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/UserManagement/IndexAddressAdmin/"+ Session["UserID"]);
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/UserManagement/IndexAddressAdmin/"+ Session["UserID"]);
            }

        }
        //End----------------------------------
        //ChangeStatusAddress by Admin
        public void ChangeStatusAddress(Guid Id)
        {
            var Result = RepUsers.RepChangeStatusAddress(Id);


            if (Result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/UserManagement/IndexAddressAdmin/" + Session["UserID"]);
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/UserManagement/IndexAddressAdmin/" + Session["UserID"]);
            }

        }
        //End----------------------------------
        //Delete Address by Admin
        public ActionResult DeleteAddressAdmin(Guid id)
        {
            var Userid = Guid.Parse(User.Identity.Name);
            if (User.Identity.IsAuthenticated)
            {
                if (Userid != null)
                {
                    var result = RepAccountDashboard.DeleteAddress(id);
                    if (result.Contains("success"))
                    {
                        TempData["JavaScriptFunction"] = IziToast.Success("عملیات با موفقیت انجام شد", "عملیات با موفقیت انجام شد");
                        Response.Redirect("/UserManagement/IndexAddressAdmin/" + Session["UserID"]);
                    }
                    else
                    {
                        TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                        Response.Redirect("/UserManagement/IndexAddressAdmin/" + Session["UserID"]);
                    }
                }
            }
            return View("Index");
        }
        //End---------------------------
        //Open Edit Address and Update it
        public ActionResult EditAddressAdmin(Guid id)
        {
            Session["UserID"] = id;
            var query = RepUsers.Edit(id);
            return View(query);
        }

        [HttpPost]
        public ActionResult AddressUpdateAdmin(Guid Id, string Address, string PostalCode, int Type, int CityRef, bool IsMain)
        {
            var Userid = Guid.Parse(User.Identity.Name);
            if (User.Identity.IsAuthenticated)
            {
                if (Userid != null)
                {
                    var result = RepUsers.RepositoryUpdateAdderss(Id, Address, PostalCode, Type, CityRef, IsMain, Userid);
                    if (result.Contains("Success"))
                    {
                        TempData["JavaScriptFunction"] = IziToast.Success("عملیات با موفقیت انجام شد", "عملیات با موفقیت انجام شد");
                        if (Session["UserLocationRef"] != null)
                        {
                            Response.Redirect("/UserManagement/IndexAddressAdmin/" + Userid);
                        }
                        else
                        {
                            Response.Redirect("/UserManagement/IndexAddressAdmin/" + Userid);
                        }

                    }
                    else
                    {
                        TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                        Response.Redirect("/UserManagement/IndexAddressAdmin/" + Userid);
                    }
                }
            }
            return View("Index");
        }

    }
}