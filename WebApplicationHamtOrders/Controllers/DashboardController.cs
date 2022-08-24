using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersDatabase.Models;
using OrdersExtentions.Extensions;
using OrdersOrders.Repository.Orders;
using OrdersOrders.ViewModels.Orders;
using SepidyarHesabExtensions.Extentions;


namespace WebApplicationHamtOrders.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {


            return View();


        }
        public ActionResult Factor()
        {


            return View();


        }

        public ActionResult Factors(string id)
        {
            Session["PicturesId"] = id;
            return View("SubmitPicture");
        }


        public ActionResult SubmitPicture()
        {


            return View();
        }


        [HttpPost]
        public ActionResult SubmitPicture(HttpPostedFileBase file)
        {
            if (Session["PicturesId"] != null)
            {
                var db = new Orders_Entities();
                var id = Guid.Parse(Session["PicturesId"].ToString());
                var filename = "Default";
                var fileExtention = "png";
                var time = DateTime.Now.Ticks.ToString();
                var code = "SP-" + time;
                var filelenght = 200;
                if (file != null)
                {
                    var userRef = Guid.NewGuid();
                    filelenght = file.ContentLength;
                    filename = "Factor_" + code;
                    fileExtention = Path.GetExtension(file.FileName);
                    string pathCombine =
                        System.Web.HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainFactor + filename + fileExtention);
                    file.SaveAs(pathCombine);
                    var qAddPic = db.Table_File_Upload.Add(new Table_File_Upload
                    {
                        Id = Guid.NewGuid(),
                        Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_File_Upload"),
                        Tables = "Table_Product",
                        Schemas = "General",
                        Ref = id,
                        FileExtensions = fileExtention,
                        FileLenght = filelenght,
                        FileUniqeName = filename + fileExtention,
                        Sort = 1,
                        IsDelete = false,
                        FileName = filename,
                        Version = 1,
                        CreatorDate = DateTime.Now,
                        CreatorRef = userRef,
                        ModifierRef = userRef,
                        ModifierDate = DateTime.Now,
                        IsOk = true,
                        IsMain = true,
                    });

                    db.SaveChanges();

                }
            }
            TempData["JavaScriptFunction"] = IziToast.Success("عملیات با موفقیت انجام شد", "عملیات با موفقیت انجام شد");
            Response.Redirect("/Dashboard/Factor");
            return View();

        }

        public ActionResult EditProfile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditProfile(string name, string family, string phone, string identification,
            string postalcode, string address, bool? sex)
        {
            var Userid = Guid.Parse(User.Identity.Name);
            if (User.Identity.IsAuthenticated)
            {
                if (Userid != null)
                {
                    var result = RepAccountDashboard.RepositoryUpdateUsers(name, family, phone, identification,
                        postalcode, address, sex, Userid);
                }
            }

            TempData["JavaScriptFunction"] = IziToast.Success("عملیات با موفقیت انجام شد", "عملیات با موفقیت انجام شد");
            return View("EditProfile");
        }

        public ActionResult Address()
        {
            return View();
        }

        public ActionResult AddressUpdate(Guid Id)
        {
            var result = RepAccountDashboard.Edit(Id);
            return View(result);
        }
        public ActionResult AddressAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GenerateAddress(VMOrders.VmOrderMangment value)
        {
            var Userid = Guid.Parse(User.Identity.Name);
            if (User.Identity.IsAuthenticated)
            {
                if (Userid != null)
                {
                    var result = RepAccountDashboard.AddNewAddress(value, Userid);
                    if (result.Contains("Success"))
                    {
                        TempData["JavaScriptFunction"] = IziToast.Success("عملیات با موفقیت انجام شد", "عملیات با موفقیت انجام شد");
                        if (Session["UserLocationRef"] != null)
                        {
                            return View("Address");
                        }
                        else
                        {
                            return View("Address");
                        }
                    }
                    else
                    {
                        TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                        if (Session["UserLocationRef"] != null)
                        {
                            return RedirectToAction("Index", "Orders");
                        }
                        return View("Address");
                    }
                }
            }
            return View("Address");
        }

        [HttpPost]
        public ActionResult GenerateAddressInOrder(VMOrders.VmOrderMangment value)
        {
            var Userid = Guid.Parse(User.Identity.Name);
            if (User.Identity.IsAuthenticated)
            {
                if (Userid != null)
                {
                    var result = RepAccountDashboard.AddNewAddress(value, Userid);
                    if (result.Contains("Success"))
                    {
                        TempData["JavaScriptFunction"] = IziToast.Success("عملیات با موفقیت انجام شد", "آدرس شما ثبت شد ، آدرس را انتخاب کنید سپس ادامه و پرداخت");
                        if (Session["UserLocationRef"] != null)
                        {
                            return RedirectToAction("Index", "Orders");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Orders");
                        }
                    }
                    else
                    {
                        TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                        if (Session["UserLocationRef"] != null)
                        {
                            return RedirectToAction("Index", "Orders");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Orders");
                        }
                    }
                }
            }
            return RedirectToAction("Index", "Orders");
        }

        [HttpPost]
        public ActionResult EditAddress(Guid id)
        {
            var result = RepAccountDashboard.RepAccountDashboardAddressById(id);
            return PartialView("P_AddressUpdate", result);
        }


        public ActionResult DeleteAddress(Guid id)
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
                        return View("Address");
                    }
                    else
                    {
                        TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                        return View("Address");
                    }
                }
            }
            return View("Address");
        }

        [HttpPost]
        public ActionResult AddressUpdate(Guid Id, string Address, string PostalCode, int Type, int CityRef, bool IsMain)
        {
            var Userid = Guid.Parse(User.Identity.Name);
            if (User.Identity.IsAuthenticated)
            {
                if (Userid != null)
                {
                    var result = RepAccountDashboard.RepositoryUpdateAdderss(Id, Address, PostalCode, Type, CityRef, IsMain);
                    if (result.Contains("Success"))
                    {
                        TempData["JavaScriptFunction"] = IziToast.Success("عملیات با موفقیت انجام شد", "عملیات با موفقیت انجام شد");
                        if (Session["UserLocationRef"] != null)
                        {
                            return View("Address");
                        }
                        else
                        {
                            return View("Address");
                        }
                    
                    }
                    else
                    {
                        TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                        return View("Address");
                    }
                }
            }
            return View("Address");
        }

        [HttpPost]
        public ActionResult AddressUpdateInOrder(Guid Id, string Address, string PostalCode, int Type, int CityRef, bool IsMain)
        {
            var Userid = Guid.Parse(User.Identity.Name);
            if (User.Identity.IsAuthenticated)
            {
                if (Userid != null)
                {
                    var result = RepAccountDashboard.RepositoryUpdateAdderss(Id, Address, PostalCode, Type, CityRef, IsMain);
                    if (result.Contains("Success"))
                    {
                        TempData["JavaScriptFunction"] = IziToast.Success("عملیات با موفقیت انجام شد", "عملیات با موفقیت انجام شد");
                        if (Session["UserLocationRef"] != null)
                        {
                           Response.Redirect("/Orders");
                        }
                        else
                        {
                            Response.Redirect("/Orders");
                        }

                    }
                    else
                    {
                        TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                        Response.Redirect("/Orders");
                    }
                }
            }

            return RedirectToAction("Index","Orders");
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(string id, string olpassword, string newpassword, string trynewpassword)
        {
            var db = new Orders_Entities();
            var idd = Guid.Parse(id);
            var query = db.Table_User.AsNoTracking().FirstOrDefault(c => c.Id == idd);
            if (query != null)
            {
                if (query.Password == olpassword)
                {
                    if (newpassword == trynewpassword)
                    {
                        query.Password = newpassword;
                        db.SaveChanges();
                    }
                }
            }
            TempData["JavaScriptFunction"] = IziToast.Success("عملیات با موفقیت انجام شد", "عملیات با موفقیت انجام شد");
            return View("Index");
        }


        [HttpPost]
        public ActionResult ChangeProfile(string id, HttpPostedFileBase file)
        {
            var db = new Orders_Entities();
            var idd = Guid.Parse(id);
            var query = db.Table_User.FirstOrDefault(c => c.Id == idd);
            if (query != null)
            {
                var filename = "Default";
                var fileExtention = "png";
                var time = DateTime.Now.Ticks.ToString();
                var code = "SP-" + time;
                var filelenght = 200;
                if (file != null)
                {
                    var userRef = Guid.NewGuid();
                    filelenght = file.ContentLength;
                    filename = "User_" + code;
                    fileExtention = Path.GetExtension(file.FileName);
                    string pathCombine =
                        System.Web.HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainProfile + filename + fileExtention);
                    file.SaveAs(pathCombine);
                    var qAddPic = db.Table_File_Upload.Add(new Table_File_Upload
                    {
                        Id = Guid.NewGuid(),
                        Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_File_Upload"),
                        Tables = "Table_User",
                        Schemas = "Account",
                        Ref = idd,
                        FileExtensions = fileExtention,
                        FileLenght = filelenght,
                        FileUniqeName = filename + fileExtention,
                        Sort = 1,
                        IsDelete = false,
                        FileName = filename,
                        Version = 1,
                        CreatorDate = DateTime.Now,
                        CreatorRef = userRef,
                        ModifierRef = userRef,
                        ModifierDate = DateTime.Now,
                        IsOk = true,
                        IsMain = true,
                    });

                    db.SaveChanges();

                }
            }
            TempData["JavaScriptFunction"] = IziToast.Success("عملیات با موفقیت انجام شد", "عملیات با موفقیت انجام شد");
            return View("Index");
        }


        public ActionResult CreatePDF(Guid id)
        {
            var db = new Orders_Entities();
            var list = new List<VMOrders.VmOrderItem>();
            var query = db.Table_Order_Item.Where(c => c.OrderRef == id).AsNoTracking().ToList();
            if (query.Count > 0)
            {
                var queryOrder = db.Table_Order.FirstOrDefault(c => c.Id == id);
                if (queryOrder != null)
                {
                    foreach (var item in query)
                    {
                        var vm = new VMOrders.VmOrderItem
                        {
                            Id = item.Id,
                            Fee = item.Fee,
                            OrderRef = item.OrderRef,
                            Quantity = item.Quantity,
                            SumRow = item.Fee * item.Quantity,
                            Code = item.Code,
                            Datetime = string.Format("{0:dddd dd MMMM yyyy - hh:mm:ss}", queryOrder.CreatorDate),
                            Name = queryOrder.Firstname,
                            Family = queryOrder.Lastname,
                            Address = queryOrder.Address,
                            Phone = queryOrder.Phone,
                            Note = queryOrder.Note,
                            PostalCode = queryOrder.PostalCode,
                            TransactionCode = queryOrder.TransactionCode,
                            DeliveryCode = queryOrder.DeliveryCode,
                            OrderCode = queryOrder.Code,
                        };

                        var queryitem = db.Table_Product.FirstOrDefault(c => c.Code == item.ItemCode);
                        if (queryitem != null)
                        {
                            vm.Title = queryitem.PrimaryTitle;
                            vm.SecTitle = queryitem.SecondaryTitle;
                            vm.ProductId = queryitem.Id;
                        }


                        var querySize = db.Table_Product_Size.FirstOrDefault(c => c.Id == item.SizeRef);
                        if (querySize != null)
                        {
                            vm.SizeTitle = querySize.SizeTitle;
                        }
                        var queryColor = db.Table_Product_Color.FirstOrDefault(c => c.Id == item.ColorRef);
                        if (queryColor != null)
                        {
                            vm.ColorTitle = queryColor.PrimaryTitle;
                        }


                        var queryfile =
                             db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == id);
                        if (queryfile != null)
                        {
                            vm.FileName = "/Static/Content/Images/Products/" + queryfile.FileName +
                                          queryfile.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Static/Content/Images/Products/Category_SP-637940016467188797.png";
                        }

                        //var queryDiscount = db.Table_DiscountOnProducts.AsNoTracking().FirstOrDefault(c => c.CategoriesRef == queryitem.CategoriesRef);
                        //if (queryDiscount != null)
                        //{
                        //    vm.Discount = queryDiscount.Discount;
                        //}


                        //var queryProductDiscount = db.Table_Discount_Product_Selection.AsNoTracking().FirstOrDefault(c => c.ProductRef == queryitem.Id);
                        //if (queryProductDiscount != null)
                        //{
                        //    var queryFindCatRef = db.Table_DiscountOnProducts.AsNoTracking().FirstOrDefault(c => c.Id == queryProductDiscount.DiscountRef);
                        //    if (queryFindCatRef != null)
                        //    {
                        //        vm.ProductDiscount = queryFindCatRef.Discount;
                        //    }
                        //}
                        list.Add(vm);

                    }
                }

                return new Rotativa.PartialViewAsPdf("Body/P_Invoice_PDF_2", list)
                {
                    FileName = "Invoice-" + DateTime.Now.Ticks.ToString() + ".pdf"
                };
            }
            else
            {
                return View("Index");
            }
        }

        public ActionResult PDF(Guid id)
        {
            var db = new Orders_Entities();
            var list = new List<VMOrders.VmOrderItem>();
            var query = db.Table_Order_Item.Where(c => c.OrderRef == id).AsNoTracking().ToList();
            if (query.Count > 0)
            {
                var queryOrder = db.Table_Order.FirstOrDefault(c => c.Id == id);
                if (queryOrder != null)
                {
                    foreach (var item in query)
                    {
                        var vm = new VMOrders.VmOrderItem
                        {
                            Id = item.Id,
                            Fee = item.Fee,
                            OrderRef = item.OrderRef,
                            Quantity = item.Quantity,
                            SumRow = item.Fee * item.Quantity,
                            Code = item.Code,
                            Datetime = string.Format("{0:dddd dd MMMM yyyy - hh:mm:ss}", queryOrder.CreatorDate),
                            Name = queryOrder.Firstname,
                            Family = queryOrder.Lastname,
                            Address = queryOrder.Address,
                            Phone = queryOrder.Phone,
                            Note = queryOrder.Note,
                            PostalCode = queryOrder.PostalCode,
                            TransactionCode = queryOrder.TransactionCode,
                            DeliveryCode = queryOrder.DeliveryCode,
                            OrderCode = queryOrder.Code,
                            Discounts = queryOrder.Discount,
                        };

                        var queryitem = db.Table_Product.FirstOrDefault(c => c.Code == item.ItemCode);
                        if (queryitem != null)
                        {
                            vm.Title = queryitem.PrimaryTitle;
                            vm.SecTitle = queryitem.SecondaryTitle;
                            vm.ProductId = queryitem.Id;
                        }


                        var querySize = db.Table_Product_Size.FirstOrDefault(c => c.Id == item.SizeRef);
                        if (querySize != null)
                        {
                            vm.SizeTitle = querySize.PrimaryTitle;
                        }
                        var queryColor = db.Table_Product_Color.FirstOrDefault(c => c.Id == item.ColorRef);
                        if (queryColor != null)
                        {
                            vm.ColorTitle = queryColor.PrimaryTitle;
                        }


                        var queryfile =
                             db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == id);
                        if (queryfile != null)
                        {
                            vm.FileName = "/Static/Content/Images/Products/" + queryfile.FileName +
                                          queryfile.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Static/Content/Images/Products/Category_SP-637940016467188797.png";
                        }

                        //var queryDiscount = db.Table_DiscountOnProducts.AsNoTracking().FirstOrDefault(c => c.CategoriesRef == queryitem.CategoriesRef);
                        //if (queryDiscount != null)
                        //{
                        //    vm.Discount = queryDiscount.Discount;
                        //}


                        //var queryProductDiscount = db.Table_Discount_Product_Selection.AsNoTracking().FirstOrDefault(c => c.ProductRef == queryitem.Id);
                        //if (queryProductDiscount != null)
                        //{
                        //    var queryFindCatRef = db.Table_DiscountOnProducts.AsNoTracking().FirstOrDefault(c => c.Id == queryProductDiscount.DiscountRef);
                        //    if (queryFindCatRef != null)
                        //    {
                        //        vm.ProductDiscount = queryFindCatRef.Discount;
                        //    }
                        //}
                        list.Add(vm);

                    }
                }

                return View("PDF", list);
                //return new Rotativa.PartialViewAsPdf("Body/P_Invoice_PDF_2", list)
                //{
                //    FileName = "Invoice-" + DateTime.Now.Ticks.ToString() + ".pdf"
                //};
            }
            else
            {
                return View("Index");
            }
        }
    }
}