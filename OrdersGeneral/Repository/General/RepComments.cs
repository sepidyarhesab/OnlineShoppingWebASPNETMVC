using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using OrdersDatabase.Models;
using OrdersGeneral.ViewModels.General;

namespace OrdersGeneral.Repository.General
{
    public class RepComments
    {
        public static List<VMComments.VMCommentsManagement> RepositoryCommentsClient(Guid id)
        {

            var list = new List<VMComments.VMCommentsManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Comments.Where(c => c.ProductRef == id && c.IsOk && !c.IsDelete && c.ParentRef == null).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VMComments.VMCommentsManagement
                        {
                            Id = item.Id,
                            Code = item.Code,
                            Name = item.Name,
                            Phone = item.Phone,
                            IsOk = item.IsOk,
                            Note = item.Note,
                            Likes = item.Likes,
                            Dislikes = item.Dislikes,
                            CreatorDate = item.CreatorDate,
                            ProductRef = item.ProductRef,
                            Rating = item.Rating

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
            }
            catch (Exception e)
            {
                return list;
            }

            return list;
        }
        public static List<VMComments.VMCommentsManagement> RepositoryCommentsClientByCommentId(Guid id)
        {

            var list = new List<VMComments.VMCommentsManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Comments.Where(c => c.ParentRef == id && c.IsOk && !c.IsDelete).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VMComments.VMCommentsManagement
                        {
                            Id = item.Id,
                            Code = item.Code,
                            Name = item.Name,
                            Phone = item.Phone,
                            IsOk = item.IsOk,
                            Note = item.Note,
                            Likes = item.Likes,
                            Dislikes = item.Dislikes,
                            CreatorDate = item.CreatorDate,
                            ProductRef = item.ProductRef,
                            Rating = item.Rating

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
            }
            catch (Exception e)
            {
                return list;
            }

            return list;
        }
        public static List<VMComments.VMCommentsManagement> RepositoryCommentsManagement()
        {

            var list = new List<VMComments.VMCommentsManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Comments.AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VMComments.VMCommentsManagement
                        {
                            Id = item.Id,
                            Code = item.Code,
                            Name = item.Name,
                            Phone = item.Phone,
                            IsOk = item.IsOk,
                            Note = item.Note,
                            Likes = item.Likes,
                            Dislikes = item.Dislikes,
                            CreatorDate = item.CreatorDate,
                            ProductRef = item.ProductRef,
                            Rating = item.Rating
                        };

                        var queryProduct = db.Table_Product.FirstOrDefault(c => c.Id == item.ProductRef);
                        if (queryProduct != null)
                        {
                            vm.ProductTitle = queryProduct.PrimaryTitle;
                        }


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
            }
            catch (Exception e)
            {
                return list;
            }

            return list;
        }


        public static List<VMComments.VMCommentsManagement> RepositoryCommentsManagementSearch(string searchnew)
        {

            var list = new List<VMComments.VMCommentsManagement>();
            try
            {
                var search = searchnew.Replace(" ", "");
                var searcha = string.IsNullOrWhiteSpace(searchnew);
                var db = new Orders_Entities();
                var query = db.Table_Comments.OrderByDescending(c => c.CreatorDate).Where(c =>c.Code == search ||c.Name.Contains(search) || c.Phone.Contains(search) || c.Note.Contains(search)).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VMComments.VMCommentsManagement
                        {
                            Id = item.Id,
                            Code = item.Code,
                            Name = item.Name,
                            Phone = item.Phone,
                            IsOk = item.IsOk,
                            Note = item.Note,
                            Likes = item.Likes,
                            Dislikes = item.Dislikes,
                            Rating = item.Rating

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
            }
            catch (Exception e)
            {
                return list;
            }

            return list;
        }


        public static string Delete(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Comments.FirstOrDefault(c => c.Id == id);
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
                                db.Table_Comments.Remove(query);
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


        public static string ChangeStatus(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Comments.FirstOrDefault(c => c.Id == id);
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

        public static string AddComments(string Name, string Phone, string Note, long Likes, Guid ProductRef, Guid Userid)
        {
            try
            {
                var db = new Orders_Entities();
                var userRef = Userid;
                var productRef = ProductRef;
                var random = new Random();
                var id = Guid.NewGuid();
                var code = "SP-" + DateTime.Now.Ticks;
                var isok = true;

                var query = db.Table_Comments.Add(new Table_Comments()
                {
                    Id = id,
                    Name = Name,
                    Note = Note,
                    Phone = Phone,
                    CreatorDate = DateTime.Now,
                    ModifierDate = DateTime.Now,
                    Code = code,
                    Version = 1,
                    IsOk = false,
                    CreatorRef = Userid,
                    ModifierRef = Userid,
                    ProductRef = productRef,
                    Rating = byte.Parse(Likes.ToString())

                });
                db.Table_Comments.Add(query);
                db.SaveChanges();

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }

    }
}