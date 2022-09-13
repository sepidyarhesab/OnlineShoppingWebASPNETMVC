using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using OrdersDatabase.Models;
using OrdersExtentions.Extensions;
using SepidyarHesabExtensions.Classes;

using OrdersOrders.ViewModels.Orders;

namespace OrdersOrders.Repository.Orders
{
    public static class RepAccountDashboard
    {
        public static List<VMOrders.VmOrderMangment> RepAccountDashboardInformation(Guid id)
        {
            var list = new List<VMOrders.VmOrderMangment>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_User.FirstOrDefault(c => c.Id == id);
                if (query != null)
                {
                    var vm = new VMOrders.VmOrderMangment
                    {
                        Name = query.Name,
                        Family = query.Family,
                        Password = query.Password,
                        Phone = query.Phone,
                        Id = query.Id,
                        Sex = query.Sex,
                        Identification = query.Identification,
                    };
                    var queryFile = db.Table_File_Upload.FirstOrDefault(c => c.Ref == id);
                    if (queryFile != null)
                    {
                        vm.Note = ServerPath.ServerPathFileUploadMainProfile + queryFile.FileName +
                                  queryFile.FileExtensions;
                    }
                    else
                    {
                        if (query.Sex == true)
                        {
                            vm.Note = "Helper/Layouts/images/avatars/Avatar-5.jpg";
                        }
                        else
                        {
                            vm.Note = "Helper/Layouts/images/avatars/Avatar-6.jpg";
                        }

                    }

                    var listaddress = RepAccountDashboardAddress(id);
                    if (listaddress.Count > 0)
                    {
                        vm.AddresList = listaddress;
                    }
                    list.Add(vm);
                    return list;
                }

            }
            catch (Exception e)
            {
                return list;
            }

            return list;

        }

        public static string[] IsAdmin(Guid id)
        {
            string[] id_name = new[] {"user", "امین رحمانی"};
            string phone = "0";
            var User_ID = Guid.Parse("5549aa2b-763c-41f1-b721-1cb9a8d7d066");
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_User.FirstOrDefault(c => c.Id == id);
                if (query != null)
                {
                    if (query.RoleRef.ToString() == "5bf16139-6715-4190-988a-4849c1a0241d")
                    {
                        id_name = new[] {"admin", query.Name + " " + query.Family};

                        phone = query.Phone;
                    }
                    else
                    {
                        id_name = new[] {"users", query.Name + " " + query.Family};

                        phone = query.Phone;
                    }
                }
                else
                {

                }
            }
            catch (Exception e)
            {
            }

            return id_name;
        }

        public static string RoleName(Guid id)
        {
            string roleName = "";
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_User.FirstOrDefault(c => c.Id == id);
                if (query != null)
                {
                    var RoleQuery = db.Table_Role.FirstOrDefault(c => c.Id == query.RoleRef);
                    if (RoleQuery != null)
                    {
                        roleName = RoleQuery.PrimaryTitle;
                    }

                }

            }
            catch (Exception e)
            {
            }

            return roleName;
        }

        public static string RepositoryUpdateUsers(string name, string family, string phone, string identification,
            string postalcode, string address, bool? sex, Guid userid)
        {
            var db = new Orders_Entities();
            var userRef = userid;
            var query = db.Table_User.FirstOrDefault(c => c.Id == userRef);
            if (query != null)
            {
                query.Name = name;
                query.Family = family;
                query.Phone = phone;
                query.Identification = identification;
                //query.Address = address;
                //query.PostalCode = postalcode;
                query.Sex = sex;
                db.SaveChanges();
            }

            return "Success";
        }

        public static List<VMOrders.VmOrderMangment> RepAccountDashboardAddress(Guid id)
        {
            var list = new List<VMOrders.VmOrderMangment>();
            try
            {
                var db = new Orders_Entities();
                var queryaddress = db.Table_Address.Where(c => c.UserRef == id).AsNoTracking().ToList();
                if (queryaddress.Count > 0)
                {
                    foreach (var item in queryaddress)
                    {
                        var vm = new VMOrders.VmOrderMangment()
                        {
                            Id = item.Id,
                            Code = item.Code,
                            Address = item.Address,
                            PostalCode = item.PostalCode,
                            Type = item.Type,
                            UserRef = id,
                        };
                        if (item.Type == 1)
                        {
                            vm.Typee = "منزل";
                        }

                        if (item.Type == 2)
                        {
                            vm.Typee = "محل کار";
                        }

                        if (item.Type == 3)
                        {
                            vm.Typee = "محل کار";
                        }

                        var querycity = db.Table_Location.FirstOrDefault(c => c.LocationId == item.CityRef);
                        if (querycity != null)
                        {
                            vm.CityTitle = querycity.Title;
                        }

                        if (item.IsMain)
                        {
                            vm.IsPayTitle = "اصلی";
                            vm.IsPayClass = "product-card__badge--yes";
                        }
                        else
                        {
                            vm.IsPayTitle = "فرعی";
                            vm.IsPayClass = "product-card__badge--no";
                        }

                        list.Add(vm);
                    }

                    return list;
                }

            }
            catch (Exception e)
            {
                return list;
            }

            return list;

        }    
        
        
        
        public static VMOrders.VmOrderMangment RepAccountDashboardAddressById(Guid id)
        {
            var list = new VMOrders.VmOrderMangment();
            try
            {
                var db = new Orders_Entities();
                var item = db.Table_Address.FirstOrDefault(c => c.Id == id);
                if (item != null)
                {
           
                        var vm = new VMOrders.VmOrderMangment()
                        {
                            Id = item.Id,
                            Code = item.Code,
                            Address = item.Address,
                            PostalCode = item.PostalCode,
                            Type = item.Type,
                            UserRef = id,
                        };
                        if (item.Type == 1)
                        {
                            vm.Typee = "منزل";
                        }

                        if (item.Type == 2)
                        {
                            vm.Typee = "محل کار";
                        }

                        if (item.Type == 3)
                        {
                            vm.Typee = "محل کار";
                        }

                        var querycity = db.Table_Location.FirstOrDefault(c => c.LocationId == item.CityRef);
                        if (querycity != null)
                        {
                            vm.CityTitle = querycity.Title;
                        }

                        if (item.IsMain)
                        {
                            vm.IsPayTitle = "اصلی";
                            vm.IsPayClass = "product-card__badge--yes";
                        }
                        else
                        {
                            vm.IsPayTitle = "فرعی";
                            vm.IsPayClass = "product-card__badge--no";
                        }

                        list = vm;
                    return list;
                }

            }
            catch (Exception e)
            {
                return list;
            }

            return list;

        }

        public static string RepositoryUpdateAdderss(Guid id, string address, string postalcode, int type, int cityref,
            bool IsMain)
        {
            var db = new Orders_Entities();
            try
            {
                var query = db.Table_Address.FirstOrDefault(c => c.Id == id);
                if (query != null)
                {
                    query.Address = address;
                    query.PostalCode = postalcode;
                    query.Type = type;
                    query.CityRef = cityref;
                    query.IsMain = IsMain;
                    query.ModifierRef = id;
                    query.ModifireDate = DateTime.Now;
                    db.SaveChanges();
                }

                return "Success";
            }
            catch (Exception e)
            {
                return "Success";
            }
        }


        public static List<VMOrders.VmOrderMangment> Edit(Guid Id)
        {
            var list = new List<VMOrders.VmOrderMangment>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Address.FirstOrDefault(c => c.Id == Id);
                if (query != null)
                {
                    var vm = new VMOrders.VmOrderMangment
                    {
                        Id = query.Id,
                        Code = query.Code,
                        Address = query.Address,
                        PostalCode = query.PostalCode,
                        Type = query.Type,
                        IsMain = query.IsMain,
                        CityRef = query.CityRef,
                    };
                    list.Add(vm);
                }

                return list;
            }
            catch (Exception e)
            {
                return list;
            }
        }

        public static List<VMOrders.VmOrderMangment> SelectionCity()
        {
            var list = new List<VMOrders.VmOrderMangment>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Location.Where(c => c.Type == "2").ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VMOrders.VmOrderMangment
                        {
                            CityRef = item.LocationId,
                            CityTitle = item.Title,
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
        
        
        public static List<VMOrders.VmOrderMangment> SelectionProvince(int id)
        {
            var list = new List<VMOrders.VmOrderMangment>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Location.Where(c => c.Parent == id).ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VMOrders.VmOrderMangment
                        {
                            CityRef = item.LocationId,
                            CityTitle = item.Title,
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


        public static string AddNewAddress(VMOrders.VmOrderMangment values, Guid userId)
        {
            try
            {
                var locaref = 0;
                if (values.CityReff != 0)
                {
                    locaref = values.CityReff;
                }
                else
                {
                    locaref = values.CityRef;
                }
                var db = new Orders_Entities();
                var random = new Random();
                var userRef = userId;
                var id = Guid.NewGuid();
                var code = "SP-" + random.Next(20000, 29999);
                var query = db.Table_Address.Add(new Table_Address()
                {
                    Id = id,
                    Code = code,
                    Address = values.Address,
                    IsMain = values.IsMain,
                    CityRef = locaref,
                    Type = values.Type,
                    PostalCode = values.PostalCode,
                    UserRef = userRef,
                    IsOk = true,
                    CreatorDate = DateTime.Now,
                    CreatorRef = userRef,
                    ModifireDate = DateTime.Now,
                    ModifierRef = userRef,
                    IsDelete = false,
                    Version = 1,
                });
                db.Table_Address.Add(query);
                db.SaveChanges();

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }

        }

        public static string DeleteAddress(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Address.FirstOrDefault(c => c.Id == id);
                if (query != null)
                {
                    switch (query.IsMain)
                    {
                        case true:
                        {
                            return "true";
                            break;

                        }
                        case false:
                        {
                            db.Table_Address.Remove(query);
                            db.SaveChanges();
                            return "success";
                            break;
                        }
                    }
                }

                return "success";
            }
            catch (Exception e)
            {

                return "Application Error : " + e.Message;

            }
        }
    }
}