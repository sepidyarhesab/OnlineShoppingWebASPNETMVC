using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using OrdersDatabase.Models;
using OrdersGeneral.ViewModels.General;
using SepidyarHesabExtensions.Classes;


namespace OrdersGeneral.Repository.General
{
    public static class RepSocialMedia
    {
        public static List<VmSocialMedia.VmSocialMediaManagement> RepositorySocialMediaManagement()
        {
            var list = new List<VmSocialMedia.VmSocialMediaManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_SocialMedia.Where(c => !c.IsDelete).OrderBy(c => c.Sort).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VmSocialMedia.VmSocialMediaManagement()
                        {
                            Id = item.Id,
                            Code = item.Code,
                            PrimaryTitle = item.PrimaryTitle,
                            SecondaryTitle = item.SecondaryTitle,
                            Class = item.Class,
                            Url = item.Url,
                            IsDelete = item.IsDelete,
                            IsOk = item.IsOk,
                            CreatorDate = item.CreatorDate,
                            CreatorRef = item.CreatorRef,
                            Sort =item.Sort,
                        };
                        var iconid = Guid.Parse(item.SecondaryTitle);
                        var queryIcon = db.Table_Static_SocialMedia.FirstOrDefault(c => c.Id == iconid);
                        if (queryIcon != null)
                        {
                            vm.Icon = queryIcon.Value;
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

        public static List<VmSocialMedia.VmSocialMediaManagement> RepositorySocialMediaManagementSearch(string searchnew)
        {
            var list = new List<VmSocialMedia.VmSocialMediaManagement>();
            try
            {
                var search = searchnew.Replace("	", "");
                var db = new Orders_Entities();
                var query = db.Table_SocialMedia.OrderByDescending(c=>c.CreatorDate).Where(c => c.Code == search ||c.PrimaryTitle.Contains(search)||c.Url.Contains(search)).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VmSocialMedia.VmSocialMediaManagement()
                        {
                            Id = item.Id,
                            Code = item.Code,
                            PrimaryTitle = item.PrimaryTitle,
                            SecondaryTitle = item.SecondaryTitle,
                            Class = item.Class,
                            Url = item.Url,
                            IsDelete = item.IsDelete,
                            IsOk = item.IsOk,
                            CreatorDate = item.CreatorDate,
                            CreatorRef = item.CreatorRef,
                            Sort = item.Sort,
                        };
                        var iconid = Guid.Parse(item.SecondaryTitle);
                        var queryIcon = db.Table_Static_SocialMedia.FirstOrDefault(c => c.Id == iconid);
                        if (queryIcon != null)
                        {
                            vm.Icon = queryIcon.Value;
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

        public static List<VmSocialMedia.VmSocialMediaManagement> RepositorySocialMedia()
        {
            var list = new List<VmSocialMedia.VmSocialMediaManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_SocialMedia.Where(c =>c.IsOk && !c.IsDelete).OrderBy(c => c.Sort).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VmSocialMedia.VmSocialMediaManagement()
                        {
                            Id = item.Id,
                            Code = item.Code,
                            PrimaryTitle = item.PrimaryTitle,
                            Url = item.Url,
                            IsDelete = item.IsDelete,
                            IsOk = item.IsOk,
                            CreatorDate = item.CreatorDate,
                            CreatorRef = item.CreatorRef
                        };
                        var idClass = Guid.Parse(item.Class);
                        var idIcon = Guid.Parse(item.SecondaryTitle);
                        var queryStaticClass = db.Table_Static_SocialMedia.FirstOrDefault(c => c.Id == idClass);
                        var queryStaticIcon = db.Table_Static_SocialMedia.FirstOrDefault(c => c.Id == idIcon);
                        if (queryStaticIcon != null && queryStaticClass != null)
                        {
                            vm.Class = queryStaticClass.Value;
                            vm.Icon = queryStaticIcon.Value;
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
        public static List<VmSocialMedia.VmSocialMediaManagement> RepositoryStaticSocialClass()
        {
            var list = new List<VmSocialMedia.VmSocialMediaManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Static_SocialMedia.Where(c => c.IsOk && c.Entities == "Class").OrderBy(c => c.Sort).ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VmSocialMedia.VmSocialMediaManagement()
                        {
                            Id = item.Id,
                            Class = item.Keys,
                            SecondaryTitle = item.Value,
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
        public static List<VmSocialMedia.VmSocialMediaManagement> RepositoryStaticSocialIcon()
        {
            var list = new List<VmSocialMedia.VmSocialMediaManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Static_SocialMedia.Where(c => c.IsOk && c.Entities == "Icon").OrderBy(c => c.Sort).ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VmSocialMedia.VmSocialMediaManagement()
                        {
                            Icon = item.Keys,
                            SecondaryTitle = item.Value,
                            Class = item.Entities,
                            Id = item.Id,
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
        public static string ChangeStatus(Guid id)
        {
            var db = new Orders_Entities();
                var query = db.Table_SocialMedia.FirstOrDefault(c => c.Id == id);
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

        public static string DeleteSocial(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_SocialMedia.FirstOrDefault(c => c.Id == id);
                if (query != null)
                {
                    switch (query.IsOk)
                    {
                        case true:
                        {
                            return "true";
                        }
                        case false:
                        {
                            db.Table_SocialMedia.Remove(query);
                            db.SaveChanges();
                            return "success";
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

        public static string AddNewRow(VmSocialMedia.VmSocialMediaManagement values, Guid userid)
        {
            try
            {
                var db = new Orders_Entities();
                var userRef = userid;
                var random = new Random();
                var id = Guid.NewGuid();
                var code = "SP-" + DateTime.Now.Ticks;
                var isok = false;

                var query = db.Table_SocialMedia.Add(new Table_SocialMedia()
                {
                    Id = id,
                    PrimaryTitle = values.PrimaryTitle,
                    Code = code,
                    Version = 1,
                    IsOk = isok,
                    CreatorRef = userRef,
                    CreatorDate = DateTime.Now,
                    ModifierDate = DateTime.Now,
                    ModifierRef = userRef,
                    Sort = values.Sort,
                    Url = values.Url,
                    SecondaryTitle = values.Icon,
                    Class = values.Class,
                });
                db.Table_SocialMedia.Add(query);
                db.SaveChanges();



                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }


        public static VmSocialMedia.VmSocialMediaManagement EditSocial(Guid? Id)
        {
            var db = new Orders_Entities();
            var query = db.Table_SocialMedia.FirstOrDefault(c => c.Id == Id);
            if (query != null)
            {
                var vm = new VmSocialMedia.VmSocialMediaManagement
                {
                    Id = query.Id,
                    PrimaryTitle = query.PrimaryTitle,
                    Code = query.Code,
                    Url = query.Url,
                    SecondaryTitle = query.SecondaryTitle,
                    Class = query.Class,
                    Sort = query.Sort,
                    Icon = query.SecondaryTitle
                };
                return vm;
            }
            else
            {
                return new VmSocialMedia.VmSocialMediaManagement();
            }
        }
        

        
    }
}