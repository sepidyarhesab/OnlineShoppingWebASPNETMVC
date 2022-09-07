using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersGeneral.Repository.General;
using OrdersInventory.Repository.Inventory;
using PagedList;
using SepidyarHesabExtensions.Extentions;


namespace WebApplicationHamtOrders.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index(int? page = 1)
        {
            var rep = new RepProducts();
            var result = rep.RepositoryMainProducts();
            if (result.Count > 0)
            {
                int pageSize = 16;
                int pageNumber = (page ?? 1);
                return View(result.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return RedirectToAction("Index", "Default");
            }
        }


        public ActionResult Id(Guid id)
        {
            ViewBag.Id = id;
            Session["ProductRef"] = id;
            var rep = new RepProducts();

            var query = rep.RepositoryMainProductsById(id);
            if (query.Count > 0)
            {
                return View(query);
            }
            return View();
        }

        public ActionResult Quantities(Guid id, Guid color, Guid size)
        {
            Session["ProductRef"] = id;
            if (id != null)
            {
                var rep = new RepProducts();
                var result = rep.RepositoryQuantitiesProducts(id, color, size);
                RedirectToAction("Quantities" ,Session["ProductRef"]);
            }

            return View("Id", Session["ProductRef"]);
        }



        [HttpPost]
        public ActionResult DropDownSize(string id)
        {
            if (Session["ProductRef"] != null)
            {
                var productRef = Guid.Parse(Session["ProductRef"].ToString());
                if (id != null)
                {
                    var rep = new RepProducts();

                    var size = rep.RepositorySizeShowInProducts(productRef, Guid.Parse(id));
                    return PartialView("SingleProduct/P_DropdownSize", size);
                }
            }
            return PartialView("SingleProduct/P_DropdownSize");
        }



        [HttpPost]
        public decimal CheckQuantity(string colorid, string sizeid)
        {
            if (Session["ProductRef"] != null)
            {
                var productRef = Guid.Parse(Session["ProductRef"].ToString());
                if (colorid != null)
                {
                    var rep = new RepProducts();
                    var size = rep.RepositoryCheckQuantityProducts(productRef, Guid.Parse(colorid),Guid.Parse(sizeid));
                    return size;
                }
            }
            return 0;
        }
        [HttpPost]
        public ActionResult CheckFee(string colorid, string sizeid)
        {
            if (Session["ProductRef"] != null)
            {
                var productRef = Guid.Parse(Session["ProductRef"].ToString());
                if (colorid != null)
                {
                    var rep = new RepProducts();
                    var Fee = rep.RepositoryCheckFeeProducts(productRef, Guid.Parse(colorid),Guid.Parse(sizeid));
                    return PartialView("SingleProduct/P_Product_Price", Fee);
                }
            }
            return PartialView("SingleProduct/P_Product_Price");
        }


        [HttpPost]
        public ActionResult Search(string Search, int? page)
        {
            var rep = new RepProducts();

            var result = rep.RepositoryMainProductSearch(Search);
            if (result.Count > 0)
            {
                Session["Search"] = Search;
                TempData["JavaScriptFunction"] = IziToast.Success(": تعداد " + result.Count + " محصول یافت شد ", "لذت ببرید ...");
                int pageSize = 12;
                int pageNumber = (page ?? 1);
                return View(result.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("محصولی یافت نشد", "محصولی یافت نشد");
                Response.Redirect("/Default");
                //return View("Index");
                return RedirectToAction("Index", "Default");
            }

        }


        public ActionResult Search( int? page)
        {
            if (Session["Search"] != null)
            {
                var rep = new RepProducts();
                var result = rep.RepositoryMainProductSearch(Session["Search"].ToString());
                if (result.Count > 0)
                {
                    TempData["JavaScriptFunction"] = IziToast.Success(": تعداد " + result.Count + " محصول یافت شد ", "لذت ببرید ...");
                    int pageSize = 12;
                    int pageNumber = (page ?? 1);
                    return View(result.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("محصولی یافت نشد", "محصولی یافت نشد");
                    Response.Redirect("/Default");
                    //return View("Index");
                    return RedirectToAction("Index", "Default");
                }
            }

            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("محصولی یافت نشد", "محصولی یافت نشد");
                Response.Redirect("/Default");
                //return View("Index");
                return RedirectToAction("Index", "Default");
            }

        }


        public void GenerateComments(string Name, string Phone, string Note, long Likes)
        {

            if (Name != "")
            {
                if (Phone != "")
                {
                    if (Note != "")
                    {
                        var ProductRef = Guid.Parse(Session["ProductRef"].ToString());
                        var Userid = Guid.Parse(User.Identity.Name);
                        var result = RepComments.AddComments(Name, Phone, Note, Likes, ProductRef, Userid);
                        if (result.Contains("Error"))
                        {
                            TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است",
                                "خطا در ایجاد سطر به دلیل : " + result);
                        }
                        else
                        {
                            TempData["JavaScriptFunction"] =
                                IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                        }
                    }
                    else
                    {

                    }
                }
                else
                {
                    TempData["JavaScriptFunction"] =
                        IziToast.Error("خطایی رخ داده است", "نام نمیتواند خالی باشد");
                }

            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نام نمیتواند خالی باشد");
            }

            Response.Redirect("/Product/Id/" + Session["ProductRef"]);

        }

    }
}