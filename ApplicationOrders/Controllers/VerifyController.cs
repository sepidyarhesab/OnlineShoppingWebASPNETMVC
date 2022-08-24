using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OrdersDatabase.Models;
using OrdersExtentions.Extensions;


namespace ApplicationOrders.Controllers
{
    public class VerifyController : Controller
    {
        // GET: Verify
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Verify(string userRef, string code, string oldCode)
        {
            if (userRef != null)
            {
                if (code == oldCode)
                {
                    FormsAuthentication.SetAuthCookie(userRef, true);
                    Response.Redirect("/Default");
                }
                else
                {
                    var db = new Orders_Entities();
                    var id = Guid.Parse(userRef);
                    var query = db.Table_User.FirstOrDefault(c => c.Id == id);
                    if (query != null)
                    {
                        var random = new Random();
                        var codee = random.Next(10000, 99999).ToString();
                        var sms = new SmsProviders();
                        sms.SendAuthentication(long.Parse(query.Phone), codee);
                        Session["LoginData"] = query.Id + "&" + codee;
                        Response.Redirect("/Verify");
                    }
                }

            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

            return RedirectToAction("Index","Default");
        }


        public ActionResult Try()
        {
            if (Session["LoginData"] != null)
            {
                var dt = Session["LoginData"].ToString();
                var sp = dt.Split('&');
                var db = new Orders_Entities();
                var id = Guid.Parse(sp[0]);
                var query = db.Table_User.FirstOrDefault(c => c.Id == id);
                if (query != null)
                {
                    var random = new Random();
                    var codee = random.Next(1000, 9999).ToString();
                    var sms = new SmsProviders();
                    sms.SendAuthentication(long.Parse(query.Phone), codee);
                    Session["LoginData"] = query.Id + "&" + codee;
                    Response.Redirect("/Verify");
                }
            }
            return RedirectToAction("Index", "Login");
        }
    }
}