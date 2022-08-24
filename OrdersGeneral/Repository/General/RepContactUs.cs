using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using OrdersDatabase.Models;
using OrdersExtentions.Extensions;
using OrdersGeneral.ViewModels.General;


namespace OrdersGeneral.Repository.General
{
    public class RepContactUs
    {
        public static List<VMContactUs.VmContactUsManagement> RepositoryContactManagements()
        {
            var list = new List<VMContactUs.VmContactUsManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_ContactUs.AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VMContactUs.VmContactUsManagement
                        {
                            Id = item.Id,
                            Code = item.Code,
                            Email= item.Email,
                            Object = item.Objects,
                            Note = item.Note,
                            Phone = item.Phone,
                            Sort = item.Sort,
                            CreatorDate = item.CreatorDate,
                            Name = item.Name,
                        };
                        if (item.IsOk)
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
                return list;
            }
            catch (Exception e)
            {
                return list;
            }

        }
        public static List<VMContactUs.VmContactUsManagement> RepositoryContactManagementsSearch(string searchnew)
        {
            var search = searchnew.Replace(" ", "");
            var searcha = string.IsNullOrWhiteSpace(searchnew);
            var list = new List<VMContactUs.VmContactUsManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_ContactUs.Where(c =>c.Code == search || c.Name.Contains(search) || c.Phone.Contains(search) || c.Objects.Contains(search)).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VMContactUs.VmContactUsManagement
                        {
                            Id = item.Id,
                            Code = item.Code,
                            Email= item.Email,
                            Object = item.Objects,
                            Note = item.Note,
                            Phone = item.Phone,
                            Sort = item.Sort,
                            CreatorDate = item.CreatorDate,
                            Name = item.Name,
                        };
                        if (item.IsOk)
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
                return list;
            }
            catch (Exception e)
            {
                return list;
            }

        }


        public static List<VMContactUs.VmContactUsManagement> RepositoryKnowing()
        {
            var list = new List<VMContactUs.VmContactUsManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_ContactUs.Where(c => c.IsOk && !c.IsDelete).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VMContactUs.VmContactUsManagement
                        {
                            Id = item.Id,
                            Code = item.Code,
                            Email = item.Email,
                            Object = item.Objects,
                            
                            Sort = item.Sort,
                            CreatorDate = item.CreatorDate,
                        };
                        
                        list.Add(vm);
                    }
                }
                return list;
            }
            catch (Exception e)
            {
                return list;
            }

        }


        #region Delete
        public static string DeleteRow(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_ContactUs.FirstOrDefault(c => c.Id == id);
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
                                db.Table_ContactUs.Remove(query);
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
        #endregion

        #region Change
        public static string ChangeStatus(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_ContactUs.FirstOrDefault(c => c.Id == id);
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
        #endregion

        public static string AddContact(string Name, string Objects, string Email, string Phone,string Note,Guid Userid)
        {
            try
            {
                var db = new Orders_Entities();
                var userRef = Userid;
                var random = new Random();
                var id = Guid.NewGuid();
                var code = "SP-" + DateTime.Now.Ticks;
                var isok = true;
                var query = db.Table_ContactUs.Add(new Table_ContactUs()
                {
                    Id = id,
                    Name = Name,
                    Note = Note,
                    Phone = Phone,
                    Code = code,
                    Objects = Objects,
                    Email = Email,
                    Version = 1,
                    IsOk = false,
                    CreatorRef = Userid,
                    ModifierRef = Userid,
                    CreatorDate = DateTime.Now,
                    ModifierDate = DateTime.Now,
                });
                db.Table_ContactUs.Add(query);
                db.SaveChanges();

                return code;
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }



    }
}
