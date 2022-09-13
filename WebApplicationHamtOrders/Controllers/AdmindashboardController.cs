using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web.Hosting;
using System.Web.Mvc;
using C1.C1Zip;
using Dynamicweb.Content.Files;
using Dynamicweb.Content.Versioning;
using Microsoft.Web.Administration;
using OrdersGeneral.Repository.General;
using OrdersDatabase.Models;


namespace WebApplicationHamtOrders.Controllers
{
    public class AdmindashboardController : Controller
    {
        // GET: Admindashboard

        public ActionResult Index()
        {
            var db = new Orders_Entities();
            if (User.Identity.IsAuthenticated)
            {
                var id = Guid.Parse(User.Identity.Name);
                var rep = RepUsers.GetRole(id);
                if (rep.Contains("Admin"))
                {
                    //return View();
                    var version = new VersionControl();
                    var result = version.ChechVersion();
                    if (result.IsOk)
                    {
                        return View();
                    }
                    else
                    {
                        string updateUrl = ConfigurationManager.AppSettings["UpdateUrl"];
                        string webUrl = ConfigurationManager.AppSettings["WebUrl"];
                        if (result.ApplicationVersionDatabase == result.ApplicationVersionAssembly)
                        {
                            if (result.DatabaseVersionDatabase == result.DatabaseVersionAssembly)
                            {

                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            var cf = new C1ZipFile();
                            var webClient = new WebClient();
                            var url = @"C:\inetpub\wwwroot\default.com\";
                            var urlback = @"C:\inetpub\wwwroot\Backup\";
                            var urlnew = url.Replace("default.com", webUrl);
                            if (Directory.Exists(urlnew))
                            {
                                if (Directory.Exists(urlback+webUrl))
                                {
                                    var zippath = urlback + webUrl + @"\Backup_" + result.ApplicationVersionDatabase + "_" + Guid.NewGuid() + ".zip";
                                    //System.IO.Compression.ZipFile.CreateFromDirectory(urlnew, zippath);
                                }
                                else
                                {
                                    Directory.CreateDirectory(urlback + webUrl);
                                    var zippath = urlback + webUrl + @"\Backup_" + result.ApplicationVersionDatabase + "_" + Guid.NewGuid() + ".zip";
                                    //ZipFile.CreateFromDirectory(urlnew, zippath);
                                }

                                //zip.Create(zippath);
                                //zip.Entries.AddFolder(urlnew, "*.*", true);
                                //// show result
                                //foreach (C1ZipEntry ze in zip.Entries)
                                //{
                                //    Console.WriteLine("{0} {1:#,##0} {2:#,##0}",
                                //        ze.FileName, ze.SizeUncompressed, ze.SizeCompressed);
                                //}


                                var durl = urlnew + @"\Update\version_v" + result.ApplicationVersionAssembly + ".zip";
                                // var urrll = "https://update.sepidyarhesab.com/update/application/SepidyarHesabDrWebApplication" + "_v" + externalip + ".zip";
                                //var urrll = "https://update.sepidyarhesab.com/update/application/SepidyarHesabDrWebApplication_v1.0.3.18.zip";
                                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                                var dl = updateUrl + result.ApplicationVersionAssembly + ".zip";
                                webClient.DownloadFile(new Uri(dl), durl);
                                string fn = durl;
                                cf.Open(fn);
                                cf.Entries.ExtractFolder(urlnew);
                                cf.Close();
                            }

                            //webClient.DownloadFile(updateUrl + result.ApplicationVersionAssembly + ".zip", HostingEnvironment.MapPath("~/" + result.ApplicationVersionAssembly + ".zip"));
                            //string zipPath = System.Web.HttpContext.Current.Server.MapPath("/Update/" + result.ApplicationVersionAssembly + ".zip");
                            //string extractPath = System.Web.HttpContext.Current.Server.MapPath("~/");
                            //ZipFile.ExtractToDirectory(zipPath, extractPath);
                        }
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            return RedirectToAction("Index", "Login");
        }

        public static void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            var a = "Success";
        }


    }
}



