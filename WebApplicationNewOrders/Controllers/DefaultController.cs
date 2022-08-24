using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using OrdersExtentions.Extensions;
using OrdersSettings.Repository.Settings;


namespace WebApplicationNewOrders.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default


        public ActionResult Index()
        {

            //var success = Crypto.Encrypt("Success");
            //var token = RepInstaller.RepositoryToken();
            //if (!token.ToString().Contains("Error"))
            //{
            //    return View();

            //}
            //else
            //{
            //    Session["TokenMessage"] = token;
            //    return RedirectToAction("Error", "Installer");
            //}
            //if (token != false)
            //{
            //    return View();

            //}
            //else
            //{
            //    return RedirectToAction("Error", "Installer");
            //}
            return View();
        }

        public ActionResult IndexFull()
        {

            var success = Crypto.Encrypt("Success");
            var token = RepInstaller.RepositoryToken();
            if (!token.ToString().Contains("Error"))
            {
                return View();

            }
            else
            {
                Session["TokenMessage"] = token;
                return RedirectToAction("Error", "Installer");
            }
            //if (token != false)
            //{
            //    return View();

            //}
            //else
            //{
            //    return RedirectToAction("Error", "Installer");
            //}
            return View();
        }


        [HttpPost]
        public ActionResult NewsLetter(string Phone)
        {
            string adminPhone = WebConfigurationManager.AppSettings["PhoneAdmin"];
            string CompanyName = WebConfigurationManager.AppSettings["CompanyName"];
            var sms = new SmsProviders();
            sms.SendAdminNewsLetter(long.Parse(Phone), adminPhone, CompanyName);
            return RedirectToAction("Index");
        }
    }
}