using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersInventory.Repository.Inventory;
using OrdersInventory.ViewModels.Inventory;
using PagedList;


namespace WebApplicationHamtOrders.Controllers
{
    public class CategoriesController : Controller
    {
        // GET: Categories
        public RepProducts rep = new RepProducts();

        public ActionResult SubCategory(Guid id, int? page)
        {
            var result = rep.RepositoryMainProductsBySubCategoreis(id);
            int pageSize = 24;
            int pageNumber = (page ?? 1);
            if (Request.Params["price"] != null)
            {
                var price = Request.Params["price"];
                switch (price)
                {
                    case "Max":
                        {
                            var results = result.OrderByDescending(c => c.Fee).ToList();
                            Session["CategoriesCount"] = results.Count();
                            Session["SubCategoriesMaxResearchFiltering"] = "Max";
                            return View(results.ToPagedList(pageNumber, pageSize));
                        }
                    case "Min":
                        {
                            var results = result.OrderBy(c => c.Fee).ToList();
                            Session["CategoriesCount"] = results.Count();
                            Session["SubCategoriesMaxResearchFiltering"] = "Min";
                            return View(results.ToPagedList(pageNumber, pageSize));
                        }
                    case "Default":
                        {
                            var results = result.OrderBy(c => c.CreatorDateTime).ToList();
                            Session["CategoriesCount"] = results.Count();
                            Session["SubCategoriesMaxResearchFiltering"] = "Default";
                            return View(results.ToPagedList(pageNumber, pageSize));
                        }
                }
            }
            else
            {
                if (Session["SubCategoriesMaxResearchFiltering"] != null)
                {
                    var price = Session["SubCategoriesMaxResearchFiltering"].ToString();
                    switch (price)
                    {
                        case "Max":
                            {
                                var results = result.OrderByDescending(c => c.Fee).ToList();
                                Session["CategoriesCount"] = results.Count();
                                Session["SubCategoriesMaxResearchFiltering"] = "Max";
                                return View(results.ToPagedList(pageNumber, pageSize));
                            }
                        case "Min":
                            {
                                var results = result.OrderBy(c => c.Fee).ToList();
                                Session["CategoriesCount"] = results.Count();
                                Session["SubCategoriesMaxResearchFiltering"] = "Min";
                                return View(results.ToPagedList(pageNumber, pageSize));
                            }
                        case "Default":
                            {
                                var results = result.OrderBy(c => c.CreatorDateTime).ToList();
                                Session["CategoriesCount"] = results.Count();
                                Session["SubCategoriesMaxResearchFiltering"] = "Default";
                                return View(results.ToPagedList(pageNumber, pageSize));
                            }
                    }
                }
                else
                {
                    Session["CategoriesCount"] = result.Count();
                    Session["SubCategoriesMaxResearchFiltering"] = null;
                    return View(result.ToPagedList(pageNumber, pageSize));
                }
            }
            return View(result.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult MainCategory(Guid id, int page = 1)
        {
            int take = 16;
            int skip = (page - 1) * take;
            ViewBag.PageId = page;
            ViewBag.PageCount = page / take;
            ViewBag.Id = id;
            var result = rep.RepositoryMainProductsByMainCategories(id).Skip(skip).Take(take).ToList();
            if (Request.Params["price"] != null)
            {
                var price = Request.Params["price"];
                switch (price)
                {
                    case "Max":
                        {
                            var results = result.OrderByDescending(c => c.Fee).ToList();
                            Session["CategoriesCount"] = results.Count();
                            Session["MainCategoriesMaxResearchFiltering"] = "Max";
                            return View(results);
                        }
                    case "Min":
                        {
                            var results = result.OrderBy(c => c.Fee).ToList();
                            Session["CategoriesCount"] = results.Count();
                            Session["MainCategoriesMaxResearchFiltering"] = "Min";
                            return View(results);
                        }
                    case "Default":
                        {
                            var results = result.OrderBy(c => c.CreatorDateTime).ToList();
                            Session["CategoriesCount"] = results.Count();
                            Session["MainCategoriesMaxResearchFiltering"] = "Default";
                            return View(results);
                        }
                }
            }
            else
            {
                if (Session["MainCategoriesMaxResearchFiltering"] != null)
                {
                    var price = Session["MainCategoriesMaxResearchFiltering"].ToString();
                    switch (price)
                    {
                        case "Max":
                            {
                                var results = result.OrderByDescending(c => c.Fee).ToList();
                                Session["CategoriesCount"] = results.Count();
                                Session["MainCategoriesMaxResearchFiltering"] = "Max";
                                return View(results);
                            }
                        case "Min":
                            {
                                var results = result.OrderBy(c => c.Fee).ToList();
                                Session["CategoriesCount"] = results.Count();
                                Session["MainCategoriesMaxResearchFiltering"] = "Min";
                                return View(results);
                            }
                        case "Default":
                            {
                                var results = result.OrderBy(c => c.CreatorDateTime).ToList();
                                Session["CategoriesCount"] = results.Count();
                                Session["MainCategoriesMaxResearchFiltering"] = "Default";
                                return View(results);
                            }
                    }
                }
                else
                {
                    Session["CategoriesCount"] = result.Count();
                    Session["MainCategoriesMaxResearchFiltering"] = null;
                    return View(result);
                }
            }
            return View(result);
        }

        public ActionResult SelectCategoreis()
        {
            return View();
        }


        #region OldCode

        //public ActionResult SubCategory(Guid id, int? page)
        //{
        //    var result = rep.RepositoryMainProductsBySubCategoreis(id).ToList();

        //    if (Session["CaregtoriesMaxResearch"] != null)
        //    {
        //        var results = result.OrderByDescending(c => c.Fee);
        //        int pageSize = 16;
        //        int pageNumber = (page ?? 1);
        //        return View(results.ToPagedList(pageNumber, pageSize));
        //    }
        //    else if (Session["CaregtoriesMinResearch"] != null)
        //    {
        //        var results = result.OrderBy(c => c.Fee);
        //        int pageSize = 16;
        //        int pageNumber = (page ?? 1);
        //        return View(results.ToPagedList(pageNumber, pageSize));
        //    }

        //    if (result.Count > 0)
        //    {
        //        int pageSize = 16;
        //        int pageNumber = (page ?? 1);
        //        return View(result.ToPagedList(pageNumber, pageSize));
        //    }

        //    return RedirectToAction("Index", "Default");
        //}

        //[HttpPost]
        //public ActionResult SubCategory(Guid id, int? page, string price)
        //{
        //    if (price != "Default")
        //    {
        //        if (price == "Max")
        //        {
        //            Session["Price"] = price;
        //            var result = rep.RepositoryMainProductsBySubCategoreis(id).ToList();
        //            Session["CategoriesCount"] = result.Count();
        //            int pageSize = 16;
        //            int pageNumber = (page ?? 1);
        //            IOrderedEnumerable<VMProduct.VmMainProduct> order = result.OrderByDescending(c => c.Fee);
        //            Session["CaregtoriesMaxResearch"] = order;
        //            return View(order.ToPagedList(pageNumber, pageSize));

        //        }
        //        else
        //        {
        //            Session["Price"] = price;
        //            var result = rep.RepositoryMainProductsBySubCategoreis(id).ToList();
        //            Session["CategoriesCount"] = result.Count();
        //            int pageSize = 16;
        //            int pageNumber = (page ?? 1);
        //            IOrderedEnumerable<VMProduct.VmMainProduct> order = result.OrderBy(c => c.Fee);
        //            Session["CaregtoriesMinResearch"] = order;
        //            return View(order.ToPagedList(pageNumber, pageSize));
        //        }
        //    }
        //    else
        //    {
        //        var result = rep.RepositoryMainProductsBySubCategoreis(id).ToList();
        //        if (result.Count > 0)
        //        {
        //            int pageSize = 16;
        //            int pageNumber = (page ?? 1);
        //            return View(result.ToPagedList(pageNumber, pageSize));
        //        }
        //        return RedirectToAction("Index", "Default");
        //    }
        //    return RedirectToAction("Index", "Default");
        //}

        //[HttpPost]
        //public ActionResult SubCategory(Guid id, int? page, Guid color)
        //{
        //    if (color != null)
        //    {
        //        Session["Color"] = color;
        //        var result = rep.RepositoryMainProductsBySubCategoreis(id).ToList();
        //        if (result.)
        //        {

        //        }
        //        Session["CategoriesCount"] = result.Count();
        //        int pageSize = 16;
        //        int pageNumber = (page ?? 1);
        //        IOrderedEnumerable<VMProduct.VmMainProduct> order = result.OrderByDescending(c => c.Fee);
        //        Session["CaregtoriesMaxResearch"] = order;
        //        return View(order.ToPagedList(pageNumber, pageSize));

        //    }
        //    else
        //    {

        //        var result = rep.RepositoryMainProductsBySubCategoreis(id).ToList();
        //        if (result.Count > 0)
        //        {
        //            int pageSize = 16;
        //            int pageNumber = (page ?? 1);
        //            return View(result.ToPagedList(pageNumber, pageSize));
        //        }
        //        return RedirectToAction("Index", "Default");

        //    }
        //    return RedirectToAction("Index", "Default");
        //}


        //public ActionResult MainCategory(Guid id, int PageId = 1)
        //{
        //    int take = 16;
        //    int skip = (PageId - 1) * take;
        //    int count = rep.RepositoryMainProductsByMainCategories(id).Count();
        //    ViewBag.PageId = PageId;
        //    ViewBag.PageCount = count / take;
        //    ViewBag.Id = id;
        //    var result = rep.RepositoryMainProductsByMainCategories(id).Skip(skip).Take(take).ToList();
        //    if (Session["MainCategoryMaxResearch"] != null)
        //    {
        //        var results = result.OrderByDescending(c => c.Fee);
        //        return View(results);
        //    }
        //    else if (Session["MainCategoryMinResearch"] != null)
        //    {
        //        var results = result.OrderBy(c => c.Fee);
        //        return View(results);
        //    }
        //    if (result.Count > 0)
        //    {
        //        return View(result);
        //    }

        //    return RedirectToAction("Index", "Default");
        //}

        //[HttpPost]
        //public ActionResult MainCategory(Guid id, string price, int PageId = 1)
        //{
        //    if (price != "Default")
        //    {
        //        if (price == "Max")
        //        {
        //            int take = 16;
        //            int skip = (PageId - 1) * take;
        //            int count = rep.RepositoryMainProductsByMainCategories(id).Count();
        //            ViewBag.PageId = PageId;
        //            ViewBag.PageCount = count / take;
        //            ViewBag.Id = id;
        //            var result = rep.RepositoryMainProductsByMainCategories(id).Skip(skip).Take(take).ToList();
        //            Session["CategoriesCount"] = result.Count();
        //            IOrderedEnumerable<VMProduct.VmMainProduct> order = result.OrderByDescending(c => c.Fee);
        //            Session["MainCategoryMaxResearch"] = order;
        //            return View(order);
        //        }
        //        else
        //        {
        //            int take = 16;
        //            int skip = (PageId - 1) * take;
        //            int count = rep.RepositoryMainProductsByMainCategories(id).Count();
        //            ViewBag.PageId = PageId;
        //            ViewBag.PageCount = count / take;
        //            ViewBag.Id = id;
        //            var result = rep.RepositoryMainProductsByMainCategories(id).Skip(skip).Take(take).ToList();
        //            Session["CategoriesCount"] = result.Count();
        //            IOrderedEnumerable<VMProduct.VmMainProduct> order = result.OrderBy(c => c.Fee);
        //            Session["MainCategoryMinResearch"] = order;
        //            return View(order);
        //        }
        //    }
        //    else
        //    {
        //        int take = 16;
        //        int skip = (PageId - 1) * take;
        //        int count = rep.RepositoryMainProductsByMainCategories(id).Count();
        //        ViewBag.PageId = PageId;
        //        ViewBag.PageCount = count / take;
        //        ViewBag.Id = id;
        //        var result = rep.RepositoryMainProductsByMainCategories(id).Skip(skip).Take(take).ToList();
        //        if (result.Count > 0)
        //        {
        //            return View(result);
        //        }
        //    }

        //    return RedirectToAction("Index", "Default");
        //}

        #endregion

    }
}