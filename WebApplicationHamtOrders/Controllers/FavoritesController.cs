using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersDatabase.Models;
using OrdersOrders.ViewModels.Orders;



namespace WebApplicationHamtOrders.Controllers
{
    public class FavoritesController : Controller
    {
        // GET: Favorites
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public decimal AddToFavorites(string id, string name)
        {
            var carts = new List<VMOrders.VmOrderSubmit>();

            if (Session["AddtoFavorites"] != null)
            {
                carts = Session["AddtoFavorites"] as List<VMOrders.VmOrderSubmit>;

            }

            if ((carts ?? throw new InvalidOperationException()).Any(p => p.Code == id))
            {
                int index = carts.FindIndex(p => p.Code == id);
                carts[index].Quantity += 1;
            }
            else
            {
                if (User.Identity.IsAuthenticated)
                {
                    var userRef = User.Identity.Name;
                    carts.Add(new VMOrders.VmOrderSubmit
                    {
                        Code = id,
                        Quantity = 1,
                        Name = name,
                        PageId = userRef,
                    });

                }
                else
                {
                    carts.Add(new VMOrders.VmOrderSubmit
                    {
                        Code = id,
                        Quantity = 1,
                        Name = name,
                    });
                }




            }

            if (User.Identity.IsAuthenticated)
            {
                var db = new Orders_Entities();
                var userref = Guid.Parse(User.Identity.Name);
                var queryFavories = db.Table_Favorites.Where(c => c.RefUser == userref).ToList();
                if (queryFavories.Count > 0)
                {
                    foreach (var favory in queryFavories)
                    {
                        if (favory.Code != id)
                        {
                            var queryadd = db.Table_Favorites.Add(new Table_Favorites
                            {
                                Id = Guid.NewGuid(),
                                Code = id,
                                CreatorDate = DateTime.Now,
                                IsOk = true,
                                CreatorRef = Guid.NewGuid(),
                                ModifierRef = Guid.NewGuid(),
                                ModifireDate = DateTime.Now,
                                PrimaryTitle = name,
                                SecendaryTitle = name,
                                RefUser = userref,
                                Version = 1,
                            });
                            db.SaveChanges();
                        }
                    }
                }
                else
                {
                    var queryadd = db.Table_Favorites.Add(new Table_Favorites
                    {
                        Id = Guid.NewGuid(),
                        Code = id,
                        CreatorDate = DateTime.Now,
                        IsOk = true,
                        CreatorRef = Guid.NewGuid(),
                        ModifierRef = Guid.NewGuid(),
                        ModifireDate = DateTime.Now,
                        PrimaryTitle = name,
                        SecendaryTitle = name,
                        RefUser = userref,
                        Version = 1,
                    });
                    db.SaveChanges();
                }
            }
            Session["AddtoFavorites"] = carts;
            return carts.Sum(p => p.Quantity);
        }

        public decimal FavoritesCount()
        {
            //var userref = User.Identity.Name;
            var carts = new List<VMOrders.VmOrderSubmit>();

            if (User.Identity.IsAuthenticated)
            {
                var db = new Orders_Entities();
                var id = Guid.Parse(User.Identity.Name);
                var query = db.Table_Favorites.Count(c => c.RefUser == id);
                return query;
            }
            else
            {
                if (Session["AddtoFavorites"] != null)
                {
                    carts = Session["AddtoFavorites"] as List<VMOrders.VmOrderSubmit>;

                }
                else
                {
                    return 0;

                }

            }

            return carts.Count;
        }

        public void DeleteFavorites(string id)
        {
            var Favorit = new List<VMOrders.VmOrderSubmit>();


            if (User.Identity.IsAuthenticated)
            {
                var db = new Orders_Entities();
                var query = db.Table_Favorites.FirstOrDefault(c => c.Code == id);
                if (query != null)
                {
                    db.Table_Favorites.Remove(query);
                    db.SaveChanges();
                }



            }

            else
            {
                if (Session["AddtoFavorites"] != null)
                {
                    Favorit = Session["AddtoFavorites"] as List<VMOrders.VmOrderSubmit>;
                    var found = Favorit.FirstOrDefault(c => c.Code == id);
                    if (found != null)
                    {


                        Favorit.Remove(found);

                    }
                }





            }
            Response.Redirect("/Favorites");
        }
    }

}



////////////////////////////////////////////////////////////

