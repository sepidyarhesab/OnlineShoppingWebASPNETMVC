using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using OrdersOrders.Repository.Orders;
using OrdersOrders.ViewModels.Orders;
using PagedList;
using Rotativa;
using Rotativa.MVC;
using SepidyarHesabExtensions.Classes;
using SepidyarHesabExtensions.Extentions;
using OrdersDatabase.Models;
using OrdersExtentions.Extensions;


namespace WebApplicationHamtOrders.Controllers
{
    public class OrdersMangementController : Controller
    {
        // GET: OrdersMangement
        public ActionResult Index()
        {
            var query = RepOrders.RepositoryListOrdersAdmin();
            return View(query);
        }

        [HttpPost]
        public ActionResult Index(string search)
        {
            if (search != "")
            {
                var result = RepOrders.RepositoryOrdersSearch(search);
                return View(result);
            }
            else
            {
                var query = RepOrders.RepositoryListOrdersAdmin();
                return View(query);

            }
        }

        public ActionResult Archive()
        {
            var query = RepOrders.RepositoryListOrdersArchive();
            return View(query);
        }

        [HttpPost]
        public ActionResult Archive(string search)
        {
            if (search != "")
            {
                var result = RepOrders.RepositoryListOrdersArchiveSearch(search);
                return View(result);
            }
            else
            {
                var query = RepOrders.RepositoryListOrdersArchive();
                return View(query);

            }
        }

        public void AddToArchive(Guid Id)
        {
            var result = RepOrders.AddToArchive(Id);
            if (result.Contains("success"))
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/OrdersMangement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/OrdersMangement");
            }
        }

        public void AddToOrder(Guid Id)
        {
            var result = RepOrders.AddToOrder(Id);

            if (result.Contains("success"))
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/OrdersMangement/Archive");
            }

            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/OrdersMangement/Archive");
            }
        }


        public ActionResult ChangeStatus(Guid id)
        {
            var db = new Orders_Entities();
            var query = db.Table_Order.FirstOrDefault(c => c.Id == id);
            if (query != null)
            {
                switch (query.Status)
                {
                    case 0:
                        {
                            query.Status = 1;
                            break;
                        }
                    case 1:
                        {
                            query.Status = 2;
                            break;
                        }
                    case 2:
                        {
                            query.Status = 3;
                            break;
                        }
                    case 3:
                        {
                            query.Status = 4;
                            break;
                        }
                    case 4:
                        {
                            query.Status = 5;
                            break;
                        }
                    case 5:
                        {
                            query.Status = 6;
                            break;
                        }
                    case 6:
                        {
                            query.Status = 7;
                            break;
                        }
                    case 7:
                        {
                            query.Status = 8;
                            break;
                        }
                    case 8:
                        {
                            query.Status = 9;
                            break;
                        }
                    case 9:
                        {
                            query.Status = 10;
                            break;
                        }
                    case 10:
                        {
                            query.Status = 0;
                            break;
                        }
                }
                db.SaveChanges();
            }
            Response.Redirect("/OrdersMangement");
            return View();
        }
        public ActionResult Invoice(Guid id)
        {
            var db = new Orders_Entities();
            Session["InvoiceManagmentId"] = id;
            var list = new List<VMOrders.VmOrderItem>();
            try
            {
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
                                Price = queryOrder.Price,
                            };

                            var queryitem = db.Table_Product.FirstOrDefault(c => c.Id == item.ItemRef);
                            if (queryitem != null)
                            {
                                vm.Title = queryitem.PrimaryTitle;
                                vm.SecTitle = queryitem.SecondaryTitle;
                                vm.ProductId = queryitem.Id;
                                vm.ProductDiscount = queryitem.Discount;
                            }

                            var querysummary = db.Table_Product_Summary.FirstOrDefault(c => c.ProductRef == queryitem.Id);
                            if (querysummary != null)
                            {
                                vm.ProductPrice = querysummary.Fee;
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
                                 db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == item.ItemRef);
                            if (queryfile != null)
                            {
                                vm.FileName = "/Static/Content/Images/Products/" + queryfile.FileName +
                                              queryfile.FileExtensions;
                            }
                            else
                            {
                                vm.FileName = "/Static/Content/Images/Products/Category_SP-637940016467188797.png";
                            }

                            list.Add(vm);

                        }
                    }

                }

            }
            catch (Exception e)
            {

            }

            return View(list);
        }
        [HttpPost]
        public ActionResult SendSms(string phone, string status)
        {
            var EnglishPhone = PersianDigits.PersianToEnglish(phone);
            var sms = new SmsProviders();
            sms.SendSmsStatus(long.Parse(EnglishPhone), status);
            TempData["JavaScriptFunction"] = IziToast.Success("پیام های نرم افزاری", "پیام با موفقیت ارسال شد");
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
                            Code = item.Code,
                            Datetime = string.Format("{0:dddd dd MMMM yyyy - hh:mm:ss}", queryOrder.CreatorDate),
                            Name = queryOrder.Firstname,
                            Family = queryOrder.Lastname,
                            Phone = queryOrder.Phone,
                            Note = queryOrder.Note,
                            PostalCode = queryOrder.PostalCode,
                            TransactionCode = queryOrder.TransactionCode,
                            DeliveryCode = queryOrder.DeliveryCode,
                            OrderCode = queryOrder.Code,
                            Discounts = queryOrder.Discount,
                            Price = queryOrder.Price,
                        };
                        try
                        {
                            var a = Guid.Parse(queryOrder.Address);
                            var queryaddress = db.Table_Address.FirstOrDefault(c => c.Id == a);
                            if (queryaddress != null)
                            {
                                vm.Address = queryaddress.Address;
                                vm.PostalCode = queryaddress.PostalCode;
                            }

                            var querycity = db.Table_Location.FirstOrDefault(c => c.LocationId == queryaddress.CityRef);
                            if (querycity != null)
                            {
                                vm.CityTitle = querycity.Title;
                            }

                        }
                        catch (Exception e)
                        {
                            vm.Address = queryOrder.Address;
                        }

                        var queryitem = db.Table_Product.FirstOrDefault(c => c.Code == item.ItemCode);
                        if (queryitem != null)
                        {
                            vm.Title = queryitem.PrimaryTitle;
                            vm.SecTitle = queryitem.SecondaryTitle;
                            vm.ProductId = queryitem.Id;
                            vm.ProductDiscount = queryitem.Discount;
                        }

                        var querysummary = db.Table_Product_Summary.FirstOrDefault(c => c.ProductRef == queryitem.Id);
                        if (querysummary != null)
                        {
                            vm.ProductPrice = querysummary.Fee;
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


        public void SendSmsPostalCode(long mobile, string postalcode)
        {
            var sms = new SmsProviders();
            sms.SendSmsPostalCode(mobile, postalcode);
            Response.Redirect("/OrdersMangement");
        }

        public ActionResult AllOrders()
        {
            var query = RepOrders.RepositoryAllOrdersAdmin();
            return View(query);
        }

        [HttpPost]
        public ActionResult AllOrders(string search)
        {
            if (search != "")
            {
                var result = RepOrders.RepositoryAllOrdersSearch(search);
                return View(result);
            }
            else
            {
                var query = RepOrders.RepositoryAllOrdersAdmin();
                return View(query);

            }
        }
        //End----------------------------------------------
        //Add items(Product) to a Order
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public void Generate(VMOrders.VmOrderMangment values)
        {
            var Userid = Guid.Parse(User.Identity.Name);
            var result = RepOrders.Generate(values, Userid);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "خطا در ایجاد سطر به دلیل : " + result);
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
            }
            Response.Redirect("AllOrders");
        }
        //End--------------------------------------------------
    }
}