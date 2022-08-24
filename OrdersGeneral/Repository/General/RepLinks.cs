using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using OrdersDatabase.Models;
using SepidyarHesabExtensions.Classes;

using OrdersGeneral.ViewModels.General;

namespace OrdersGeneral.Repository.General
{
    public static class RepLinks
    {


        #region Add
        public static string AddNewRow(VMLinks.VMMainLinksManagementGenerate values, Guid userid)
        {
            try
            {
                var db = new Orders_Entities();
                var userRef = userid;
                var random = new Random();
                var id = Guid.NewGuid();
                var code = "SP-" + DateTime.Now.Ticks;
                var isok = false;
                switch (values.IsOk)
                {
                    case 1:
                        {
                            isok = true;
                            break;
                        }
                }
                var query = db.Table_Links.Add(new Table_Links()
                {
                    Id = id,
                    PrimaryTitle = values.PrimaryTitle,
                    SecendryTitle = values.SecondaryTitle,
                    Url = values.Url,
                    CreatorDate = DateTime.Now,
                    ModifireDate = DateTime.Now,
                    Code = code,
                    Version = 1,
                    IsOk = isok,
                    CreatorRef = userRef,
                    ModifierRef = userRef,
                    Sort = values.Sort,
                });
                db.Table_Links.Add(query);
                db.SaveChanges();



                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }



        #endregion




        #region Edit

        public static string RepositoryMainLinksManagemenetEditById(VMLinks.VMMainLinksManagement values, HttpPostedFileBase files, Guid userid)
        {
            try
            {
                var db = new Orders_Entities();
                var userRef = userid;
                var query = db.Table_Links.FirstOrDefault(c => c.Id == values.Id);
                if (query != null)
                {
                    query.PrimaryTitle = values.PrimaryTitle;
                    query.SecendryTitle = values.SecondaryTitle;
                    query.Url = values.Url;
                    query.ModifireDate = DateTime.Now;
                    query.ModifierRef = userRef;
                    query.Version++;
                    query.Sort = values.Sort;

                    switch (values.IsOk)
                    {
                        case 1:
                        {
                            query.IsOk = true;
                            break;
                        }   
                        case 0:
                        {

                            query.IsOk = false;
                            break;
                        }
                    }


                    db.SaveChanges();
                }
                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }





        }


        #endregion



     

        public static List<VMLinks.VMMainLinks> RepositoryMainLinks()
        {
            var list = new List<VMLinks.VMMainLinks>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Links.Where(c => c.IsOk).AsNoTracking().ToList();
                if (query.Count>0)
                {
                    foreach (var linkse in query)
                    {
                        var vm = new VMLinks.VMMainLinks
                        {
                            Id= linkse.Id,
                            PrimaryTitle = linkse.PrimaryTitle,
                            SecondaryTitle = linkse.SecendryTitle,
                            Url = linkse.Url,
                        };
                        list.Add(vm);
                    }
                }
            }
            catch (Exception e)
            {
                
            }

            return list;
        }

        public static List<VMLinks.VMMainLinksManagement> RepositoryMainLinksManagement()
        {
            var list = new List<VMLinks.VMMainLinksManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Links.AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var linkse in query)
                    {
                        var vm = new VMLinks.VMMainLinksManagement
                        {
                             
                            Id = linkse.Id,
                            PrimaryTitle = linkse.PrimaryTitle,
                            SecondaryTitle = linkse.SecendryTitle,
                            Url = linkse.Url,
                            Code = linkse.Code,
                            Sort = linkse.Sort,
                            CreatorDateTime = linkse.CreatorDate,
                            
                        };

                        if (linkse.IsOk)
                        {
                            vm.IsOkClass = "btn btn-success";
                            vm.IsOkTitle = "فعال";
                        }
                        else
                        {
                            vm.IsOkClass = "btn btn-danger";
                            vm.IsOkTitle = "غیر فعال";
                        }
                        list.Add(vm);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return list;
        }
        public static List<VMLinks.VMMainLinksManagement> RepositoryMainLinksManagementSearch(string searchnew)
        {
            var list = new List<VMLinks.VMMainLinksManagement>();
            try
            {
                var search = searchnew.Replace("	", "");
                var searcha = string.IsNullOrWhiteSpace(searchnew);
                var db = new Orders_Entities();
                var query = db.Table_Links.OrderByDescending(c=>c.CreatorDate).Where(c=>c.Code == search || c.PrimaryTitle.Contains(search)|| c.SecendryTitle.Contains(search)||c.Url.Contains(search)).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var linkse in query)
                    {
                        var vm = new VMLinks.VMMainLinksManagement
                        {

                            Id = linkse.Id,
                            PrimaryTitle = linkse.PrimaryTitle,
                            SecondaryTitle = linkse.SecendryTitle,
                            Url = linkse.Url,
                            Code = linkse.Code,
                            Sort = linkse.Sort,
                            CreatorDateTime = linkse.CreatorDate,

                        };

                        if (linkse.IsOk)
                        {
                            vm.IsOkClass = "btn btn-success";
                            vm.IsOkTitle = "فعال";
                        }
                        else
                        {
                            vm.IsOkClass = "btn btn-danger";
                            vm.IsOkTitle = "غیر فعال";
                        }
                        list.Add(vm);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return list;
        }

        public static List<VMLinks.VMMainLinksManagement> RepositoryMainLinksManagementById(Guid id)
        {
            var list = new List<VMLinks.VMMainLinksManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Links.FirstOrDefault(c => c.Id == id);
                if (query!= null)
                {
                    var vm = new VMLinks.VMMainLinksManagement
                    {
                        Id = query.Id,
                        PrimaryTitle = query.PrimaryTitle,
                        SecondaryTitle = query.SecendryTitle,
                        Url = query.Url,
                        Code = query.Code,
                        Sort = query.Sort,
                        CreatorDateTime = query.CreatorDate,
                        
                    };

                    if (query.IsOk)
                    {
                        vm.IsOkClass = "btn btn-success";
                        vm.IsOkTitle = "فعال";
                    }
                    else
                    {
                        vm.IsOkClass = "btn btn-danger";
                        vm.IsOkTitle = "غیر فعال";
                    }
                    list.Add(vm);
                }
            }
            catch (Exception e)
            {

            }

            return list;
        }









        public static string ChangeStatus(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Links.FirstOrDefault(c => c.Id == id);
                if (query != null)
                {
                    switch (query.IsOk)
                    {
                        case true:
                        {
                            query.IsOk = false;
                            db.SaveChanges();
                            break;
                        }
                        case false:
                        {
                            query.IsOk = true;
                            db.SaveChanges();
                            break;
                        }
                    }
                }

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }










        public static string DeleteRow(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Links.FirstOrDefault(c => c.Id == id);
                if (query != null)
                {
                    switch (query.IsOk)
                    {
                        case true:
                        {
                            return "TRUE";
                        }
                        case false:
                        {
                            db.Table_Links.Remove(query);
                            db.SaveChanges();
                            break;
                        }
                    }
                }

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }
    }
}