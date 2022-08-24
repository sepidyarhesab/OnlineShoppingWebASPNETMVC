using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersGeneral.Repository.General;
using WebApplicationOrders.Database;


namespace WebApplicationOrders.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        public ActionResult Index(int PageId = 1)
        {
            int take =6;
            int skip = (PageId - 1) * take;
            int count = RepBlog.RepositoryMainBlogManagement().Count();
            ViewBag.PageId = PageId;
            ViewBag.PageCount = count / take;
            var blog = RepBlog.RepositoryMainBlogManagement().Skip(skip).Take(take).ToList();
            return View(blog);
        }


        [HttpPost]
        public ActionResult Index(string SearchKey)
        {
            ViewBag.SearchKey = SearchKey;
            var query = RepBlog.RepositorySearchWithPrimaryTitle(SearchKey);
            return View(query);
        }

        public ActionResult MoreBlog(Guid id)
        {
            var query = RepBlog.RepositoryMainBlogById(id);
            if (query.Count > 0)
            {
                return View(query);
            }
            return View();
        }
        [HttpPost]
        public ActionResult MoreBlog(string SearchKey)
        {
            ViewBag.SearchKey = SearchKey;
            var query = RepBlog.RepositorySearchWithPrimaryTitle(SearchKey);
            return View(query);
        }

        public ActionResult BlogSubCategory(Guid id, int PageId = 1)
        {
            int take = 6;
            int skip = (PageId - 1) * take;
            int count = RepBlog.RepositoryMainBlogBySubCategoreis(id).Count();
            ViewBag.PageId = PageId;
            ViewBag.PageCount = count / take;
            ViewBag.Id = id;
            var result = RepBlog.RepositoryMainBlogBySubCategoreis(id).Skip(skip).Take(take).ToList();
            if (result.Count > 0)
            {
                return View(result);
            }
            return RedirectToAction("Index", "Default");
        }
        [HttpPost]
        public ActionResult BlogSubCategory(string SearchKey)
        {
            ViewBag.SearchKey = SearchKey;
            var query = RepBlog.RepositorySearchWithPrimaryTitle(SearchKey);
            return View(query);
        }

        public ActionResult BlogMainCategory(Guid id, int PageId = 1)
        {
            int take = 6;
            int skip = (PageId - 1) * take;
            int count = RepBlog.RepositoryMainBlogByMainCategories(id).Count();
            ViewBag.PageId = PageId;
            ViewBag.PageCount = count / take;
            ViewBag.Id = id;
            var result = RepBlog.RepositoryMainBlogByMainCategories(id).Skip(skip).Take(take).ToList();
            if (result.Count > 0)
            {
                return View(result);
            }

            return RedirectToAction("Index", "Default");




        }
        [HttpPost]
        public ActionResult BlogMainCategory(string SearchKey)
        {
            ViewBag.SearchKey = SearchKey;
            var query = RepBlog.RepositorySearchWithPrimaryTitle(SearchKey);
            return View(query);
        }
    }
}