using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersInventory.Repository.Inventory;
using PagedList;

namespace ApplicationOrders.Controllers
{
    public class CategoriesController : Controller
    {
        // GET: Categories
        public RepProducts rep = new RepProducts();

        public ActionResult SubCategory(Guid id, int? page)
        {
            var result = rep.RepositoryMainProductsBySubCategoreis(id).ToList();
            if (result.Count > 0)
            {
                int pageSize = 16;
                int pageNumber = (page ?? 1);
                return View(result.ToPagedList(pageNumber, pageSize));
            }

            return RedirectToAction("Index", "Default");
        }

        public ActionResult MainCategory(Guid id, int PageId = 1)
        {
            int take = 16;
            int skip = (PageId - 1) * take;
            int count = rep.RepositoryMainProductsByMainCategories(id).Count();
            ViewBag.PageId = PageId;
            ViewBag.PageCount = count / take;
            ViewBag.Id = id;
            var result = rep.RepositoryMainProductsByMainCategories(id).Skip(skip).Take(take).ToList();
            if (result.Count > 0)
            {
                return View(result);
            }

            return RedirectToAction("Index", "Default");
        }
    }
}