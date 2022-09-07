using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using OrdersDatabase.Models;
using OrdersExtentions.Extensions;
using OrdersGeneral.ViewModels.General;

namespace OrdersGeneral.Repository.General
{
    public class RepBlog
    {
        #region category
        public static List<VMBlogs.ViewModelCategories> RepositoryBlogCategoryManagment()
        {
            var list = new List<VMBlogs.ViewModelCategories>();
            try
            {
                var db = new Orders_Entities();

                var query = db.Table_Blog_Category.OrderBy(c => c.Sort).AsNoTracking()
                    .ToList();

                if (query.Count > 0)
                {

                    foreach (var category in query)
                    {

                        var vm = new VMBlogs.ViewModelCategories()
                        {
                            Id = category.Id,
                            PrimaryTitle = category.PrimaryTitle,
                            ParentRef = category.ParentRef,
                            IsMain = category.IsMain,
                            Code = category.Code,
                            IsDelete = category.IsDelete,
                            SecondaryTitle = category.SecondaryTitle,
                            Sort = category.Sort,
                            IsOk = category.IsOk,
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
        public static List<VMBlogs.ViewModelCategories> RepositoryBlogCategoryManagmentDropDown()
        {
            var list = new List<VMBlogs.ViewModelCategories>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Blog_Category
                    .ToList();

                if (query.Count > 0)
                {
                    foreach (var category in query)
                    {
                        var vm = new VMBlogs.ViewModelCategories()
                        {
                            Id = category.Id,
                            PrimaryTitle = category.PrimaryTitle,
                            ParentRef = category.ParentRef,
                            IsMain = category.IsMain,
                            Code = category.Code,
                            IsDelete = category.IsDelete,
                            SecondaryTitle = category.SecondaryTitle,
                        };


                        list.Add(vm);
                    }


                    var nulll = new VMBlogs.ViewModelCategories()
                    {
                        Id = Guid.Empty,
                        PrimaryTitle = "--- بدون دسته بندی ---",
                        Code = "1",
                    };


                    list.Add(nulll);


                }


            }
            catch (Exception e)
            {

            }

            return list;
        }
        public static string AddNewBlogCategories(VMBlogs.ViewModelCategories values, Guid UserId)
        {
            try
            {
                var db = new Orders_Entities();
                var userRef = UserId;
                var random = new Random();
                var id = Guid.NewGuid();
                var code = "SP-" + random.Next(20000, 29999);
                var isok = false;
                var parentRef = values.ParentRef;


                if (values.ParentRef == Guid.Empty)
                {
                    parentRef = null;
                }

                switch (values.IsOk)
                {
                    case true:

                        {
                            isok = true;
                            break;
                        }
                }
                var query = db.Table_Blog_Category.Add(new Table_Blog_Category()
                {
                    Id = id,
                    PrimaryTitle = values.PrimaryTitle,
                    SecondaryTitle = values.SecondaryTitle,
                    CreatorDate = DateTime.Now,
                    ModifierDate = DateTime.Now,
                    Code = code,
                    Version = 1,
                    IsOk = isok,
                    CreatorRef = userRef,
                    ModifierRef = userRef,
                    IsDelete = false,
                    IsMain = values.IsMain,
                    ParentRef = parentRef,
                    TertiaryTitle = values.SecondaryTitle,
                    Sort = values.Sort,
                    Url = values.Url,
                });
                db.Table_Blog_Category.Add(query);
                db.SaveChanges();

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }
        public static VMBlogs.ViewModelCategories RepositoryBlogCategoryMangmentByid(Guid Id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Blog_Category.FirstOrDefault(c => c.Id == Id);
                if (query != null)
                {
                    var vm = new VMBlogs.ViewModelCategories()
                    {
                        Id = query.Id,
                        Code = query.Code,
                        IsOk = query.IsOk,
                        PrimaryTitle = query.PrimaryTitle,
                        SecondaryTitle = query.SecondaryTitle,
                        ParentRef = query.ParentRef,
                        CreatorDate = query.CreatorDate,
                        IsDelete = query.IsDelete,
                        IsMain = query.IsMain,
                        ModifierDate = query.ModifierDate,
                        Sort = query.Sort,
                    };

                    return vm;
                }

            }
            catch (Exception e)
            {

            }
            return new VMBlogs.ViewModelCategories();
        }
        public static string RepositoryBlogCategoryMangmentEditById(VMBlogs.ViewModelCategories values, Guid Userid)
        {
            try
            {
                var db = new Orders_Entities();
                var userRef = Userid;
                var query = db.Table_Blog_Category.FirstOrDefault(c => c.Id == values.Id);
                var parentRef = values.ParentRef;

                if (values.ParentRef == Guid.Empty)
                {
                    parentRef = null;
                }

                if (query != null)
                {
                    query.PrimaryTitle = values.PrimaryTitle;
                    query.SecondaryTitle = values.SecondaryTitle;
                    query.ModifierDate = DateTime.Now;
                    query.ModifierRef = userRef;
                    query.Version++;
                    query.ParentRef = parentRef;
                    query.IsMain = values.IsMain;
                    query.Sort = values.Sort;


                    db.SaveChanges();
                }

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }
        public static string DeleteBlogCategory(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Blog_Category.FirstOrDefault(c => c.Id == id);
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
                                db.Table_Blog_Category.Remove(query);
                                db.SaveChanges();
                                return "success";
                            }
                    }
                }

                return "else";
            }
            catch (Exception e)
            {

                return "Application Error : " + e.Message;

            }
        }
        public static string ChangeBlogCategoryStatus(Guid Id)
        {
            var db = new Orders_Entities();
            var Result = db.Table_Blog_Category.FirstOrDefault(c => c.Id == Id);
            if (Result != null)
            {

                switch (Result.IsOk)
                {
                    case true:
                        {
                            Result.IsOk = false;
                            break;
                        }
                    case false:
                        {
                            Result.IsOk = true;
                            break;
                        }
                }


            }

            db.SaveChanges();

            return "Sucsess";

        }
        public static List<VMBlogs.ViewModelCategories> RepositoryBlogMainCategoreis()
        {
            var list = new List<VMBlogs.ViewModelCategories>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Blog_Category.Where(c => c.IsMain).OrderBy(c => c.Sort)
                    .ToList();
                if (query.Count > 0)
                {
                    foreach (var category in query)
                    {
                        var vm = new VMBlogs.ViewModelCategories
                        {
                            Id = category.Id,
                            PrimaryTitle = category.PrimaryTitle,
                            ParentRef = category.ParentRef,
                            Code = category.Code,
                        };
                        var listsubcategorie = new List<VMBlogs.ViewModelCategories>();

                        var querySub = db.Table_Blog_Category
                            .Where(c => c.ParentRef == category.Id && c.IsOk && !c.IsDelete && !c.IsMain).AsNoTracking()
                            .ToList();
                        if (querySub.Count > 0)
                        {
                            foreach (var productCategory in querySub)
                            {
                                var vmm = new VMBlogs.ViewModelCategories()
                                {
                                    Id = productCategory.Id,
                                    PrimaryTitle = productCategory.PrimaryTitle,
                                };
                                listsubcategorie.Add(vmm);
                            }

                            vm.ViewModelSubCategoreis = listsubcategorie;
                        }


                        list.Add(vm);
                    }
                }

                return list;
            }
            catch (Exception e)
            {

            }

            return list;
        }

        public static List<VMBlogs.VMBlog> RepositoryMainBlogByMainCategories(Guid id)
        {
            var list = new List<VMBlogs.VMBlog>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Blog_Category.Where(c => (c.IsOk && c.ParentRef == id) || c.Id == id)
                    .OrderBy(c => c.Sort).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var selection in query)
                    {
                        var SubId = selection.Id;
                        var queryProduct = db.Table_Blog.Where(c => c.CategoryRef == SubId)
                               .OrderByDescending(c => c.ModifierDate).AsNoTracking().ToList();
                        if (queryProduct.Count > 0)
                        {
                            foreach (var blog in queryProduct)
                            {
                                var vm = new VMBlogs.VMBlog()
                                {
                                    Id = blog.Id,
                                    Code = blog.Code,
                                    IsOk = blog.IsOk,
                                    PrimaryTitle = blog.PrimaryTitle,
                                    SecondaryTitle = blog.SecondaryTitle,
                                    Url = blog.Url,
                                    Description = blog.Description,
                                    Sort = blog.Sort,
                                    Summary = blog.summary,
                                    CreatorDate = blog.CreatorDate,
                                    Tag = blog.Tag,
                                };

                                var queryfile =
                                    db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == blog.Id && c.IsMain);
                                if (queryfile != null)
                                {
                                    vm.FileName = "/Static/Content/Images/BlogImages/" + queryfile.FileName +
                                                  queryfile.FileExtensions;
                                }
                                else
                                {
                                    vm.FileName = "/Static/Content/Images/";
                                }

                                var queryCategories =
                                    db.Table_Blog_Category.AsNoTracking().FirstOrDefault(c => c.Id == blog.CategoryRef);
                                if (queryCategories != null)
                                {
                                    vm.CategoryTitle = queryCategories.PrimaryTitle;
                                    vm.CategoryRef = queryCategories.Id;
                                }
                                list.Add(vm);
                            }
                        }

                    }
                }
            }
            catch (Exception e)
            {

            }

            return list;
        }

        public static List<VMBlogs.VMBlog> RepositoryMainBlogBySubCategoreis(Guid id)
        {
            var list = new List<VMBlogs.VMBlog>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Blog.Where(c => c.IsOk && c.CategoryRef == id)
                    .OrderByDescending(c => c.ModifierDate).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var blog in query)
                    {
                        var vm = new VMBlogs.VMBlog()
                        {
                            Id = blog.Id,
                            Code = blog.Code,
                            IsOk = blog.IsOk,
                            PrimaryTitle = blog.PrimaryTitle,
                            SecondaryTitle = blog.SecondaryTitle,
                            Url = blog.Url,
                            Description = blog.Description,
                            CreatorDate = blog.CreatorDate,
                            Summary = blog.summary,
                            Sort = blog.Sort,
                            Tag = blog.Tag,
                        };

                        var queryfile =
                            db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == blog.Id && c.IsMain);
                        if (queryfile != null)
                        {
                            vm.FileName = "/Static/Content/Images/BlogImages/" + queryfile.FileName +
                                          queryfile.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Static/Content/Images/";
                        }



                        var queryCategories =
                            db.Table_Blog_Category.FirstOrDefault(c => c.Id == blog.CategoryRef && c.IsOk && !c.IsDelete);
                        if (queryCategories != null)
                        {
                            vm.CategoryTitle = queryCategories.PrimaryTitle;
                            vm.CategoryRef = queryCategories.Id;

                        }
                        var queryUsers =
                            db.Table_User.FirstOrDefault(c => c.Id == blog.CreatorRef && c.IsOk && !c.IsDelete);
                        if (queryCategories != null)
                        {
                            vm.CreatorName = queryUsers.Name + " " + queryUsers.Family;
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

        #endregion

        #region blog

        public static List<VMBlogs.VMBlog> RepositoryRelatedBlog(Guid? CategoryRef)
        {

            var list = new List<VMBlogs.VMBlog>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Blog
                    .Where(c => c.CategoryRef == CategoryRef).Take(2).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var blog in query)
                    {
                        var vm = new VMBlogs.VMBlog()
                        {
                            Id = blog.Id,
                            Code = blog.Code,
                            IsOk = blog.IsOk,
                            PrimaryTitle = blog.PrimaryTitle,
                            Url = blog.Url,
                            CreatorDate = blog.CreatorDate,
                            Description = blog.Description,
                            Summary = blog.summary,

                        };

                        var queryfile =
                            db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == blog.Id);
                        if (queryfile != null)
                        {
                            vm.FileName = "/Static/Content/Images/BlogImages/" + queryfile.FileName +
                                          queryfile.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Static/Content/Images/";
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
        public static string RepositoryMainBlogManagemenetEditById(VMBlogs.VMBlog values, HttpPostedFileBase files, Guid UserId)
        {
            try
            {
                var db = new Orders_Entities();
                var userRef = UserId;
                var query = db.Table_Blog.FirstOrDefault(c => c.Id == values.Id);
                if (query != null)
                {
                    query.PrimaryTitle = values.PrimaryTitle;
                    query.SecondaryTitle = values.SecondaryTitle;
                    query.Url = values.Url;
                    query.ModifierDate = DateTime.Now;
                    query.ModifierRef = userRef;
                    query.Version++;
                    query.Sort = values.Sort;
                    query.Description = values.Description;
                    query.summary = values.Summary;

                    db.SaveChanges();

                    var filename = "Default";
                    var fileExtention = "png";
                    var time = DateTime.Now.Ticks.ToString();
                    var code = "SP-" + time;
                    var filelenght = 200;
                    if (files != null)
                    {
                        var qPics = db.Table_File_Upload.Where(c => c.Ref == query.Id && c.IsMain).ToList();
                        if (qPics.Count > 0)
                        {
                            foreach (var fileUpload in qPics)
                            {
                                if (File.Exists(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainBlog + fileUpload.FileName + fileUpload.FileExtensions)))
                                {
                                    File.Delete(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainBlog + fileUpload.FileName + fileUpload.FileExtensions));
                                }
                                db.Table_File_Upload.Remove(fileUpload);
                                db.SaveChanges();
                            }
                        }
                        filelenght = files.ContentLength;
                        filename = "Slider_" + code;
                        fileExtention = Path.GetExtension(files.FileName);
                        string pathCombine =
                            System.Web.HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainBlog + filename + fileExtention);
                        files.SaveAs(pathCombine);
                        var qadd = db.Table_File_Upload.Add(new Table_File_Upload
                        {
                            Id = Guid.NewGuid(),
                            Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_File_Upload"),
                            Tables = "Table_Blog",
                            Schemas = "General",
                            Ref = query.Id,
                            FileExtensions = fileExtention,
                            FileLenght = filelenght,
                            FileUniqeName = filename + fileExtention,
                            Sort = 1,
                            IsDelete = false,
                            FileName = filename,
                            Version = 1,
                            CreatorDate = DateTime.Now,
                            CreatorRef = userRef,
                            ModifierRef = userRef,
                            ModifierDate = DateTime.Now,
                            IsOk = true,
                            IsMain = true,
                        });
                        db.Table_File_Upload.Add(qadd);
                        db.SaveChanges();
                    }
                }


                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }



        }

        public static string DeleteBlog(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Blog.FirstOrDefault(c => c.Id == id);
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
                                var qPics = db.Table_File_Upload.Where(c => c.Ref == query.Id).ToList();
                                if (qPics.Count > 0)
                                {
                                    foreach (var fileUpload in qPics)
                                    {
                                        if (File.Exists(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainBlog + fileUpload.FileName + fileUpload.FileExtensions)))
                                        {
                                            File.Delete(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainBlog + fileUpload.FileName + fileUpload.FileExtensions));
                                        }
                                        db.Table_File_Upload.Remove(fileUpload);
                                        db.SaveChanges();
                                    }
                                }
                                db.Table_Blog.Remove(query);
                                db.SaveChanges();
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

        public static string ChangeBlogStatus(Guid id)
        {
            var db = new Orders_Entities();
            var Result = db.Table_Blog.FirstOrDefault(c => c.Id == id);
            if (Result != null)
            {

                switch (Result.IsOk)
                {
                    case true:
                        {
                            Result.IsOk = false;
                            break;
                        }
                    case false:
                        {
                            Result.IsOk = true;
                            break;
                        }
                }


            }

            db.SaveChanges();

            return "Sucsess";
        }

        public static string AddNewBlogRow(VMBlogs.VMBlog values, HttpPostedFileBase file, Guid Userid)
        {
            try
            {
                var db = new Orders_Entities();
                var userRef = Userid;
                var random = new Random();
                var id = Guid.NewGuid();
                var code = "SP-" + random.Next(10000, 99999);
                var isok = false;
                switch (values.IsOk)
                {
                    case true:

                        {
                            isok = true;
                            break;
                        }
                }

                var query = db.Table_Blog.Add(new Table_Blog()
                {
                    Id = id,
                    PrimaryTitle = values.PrimaryTitle,
                    SecondaryTitle = values.SecondaryTitle,
                    Url = values.Url,
                    CreatorDate = DateTime.Now,
                    ModifierDate = DateTime.Now,
                    Code = code,
                    Version = 1,
                    IsOk = isok,
                    CreatorRef = userRef,
                    ModifierRef = userRef,
                    IsDelete = false,
                    Click = 1,
                    Description = values.Description,
                    Sort = values.Sort,
                    summary = values.Summary,
                    Tag = values.Tag,
                    CategoryRef = values.CategoryRef,
                });

                db.Table_Blog.Add(query);
                db.SaveChanges();


                var filename = "Default";
                var fileExtention = "png";
                var filelenght = 200;
                if (file != null)
                {

                    filelenght = file.ContentLength;
                    filename = "Blog_" + code;
                    fileExtention = Path.GetExtension(file.FileName);
                    string pathCombine =
                        HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainBlog + filename +
                                                           fileExtention);
                    file.SaveAs(pathCombine);
                    var qPics = db.Table_File_Upload.Where(c => c.Ref == id).AsNoTracking().ToList();
                    if (qPics.Count != 0)
                    {
                        foreach (var item in qPics)
                        {
                            var filee = HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainProduct +
                                                                           item.FileName + item.FileExtensions);
                            File.Delete(filee);
                            db.Table_File_Upload.Remove(item);
                            db.SaveChanges();
                        }

                        var qAddPic = db.Table_File_Upload.Add(new Table_File_Upload());
                        qAddPic.Id = Guid.NewGuid();
                        qAddPic.Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_File_Upload");
                        qAddPic.Tables = "Table_Blog";
                        qAddPic.Schemas = "General";
                        qAddPic.Ref = id;
                        qAddPic.FileExtensions = fileExtention;
                        qAddPic.FileLenght = filelenght;
                        qAddPic.FileUniqeName = filename + fileExtention;
                        qAddPic.Sort = 1;
                        qAddPic.IsDelete = false;
                        qAddPic.FileName = filename;
                        qAddPic.Version = 1;
                        qAddPic.CreatorDate = DateTime.Now;
                        qAddPic.CreatorRef = userRef;
                        qAddPic.ModifierRef = userRef;
                        qAddPic.ModifierDate = DateTime.Now;
                        qAddPic.IsOk = true;
                        qAddPic.IsMain = true;
                        db.SaveChanges();
                    }
                    else
                    {
                        var qAddPic = db.Table_File_Upload.Add(new Table_File_Upload());
                        qAddPic.Id = Guid.NewGuid();
                        qAddPic.Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_File_Upload");
                        qAddPic.Tables = "Table_Product";
                        qAddPic.Schemas = "General";
                        qAddPic.Ref = id;
                        qAddPic.FileExtensions = fileExtention;
                        qAddPic.FileLenght = filelenght;
                        qAddPic.FileUniqeName = filename + fileExtention;
                        qAddPic.Sort = 1;
                        qAddPic.IsDelete = false;
                        qAddPic.FileName = filename;
                        qAddPic.Version = 1;
                        qAddPic.CreatorDate = DateTime.Now;
                        qAddPic.CreatorRef = userRef;
                        qAddPic.ModifierRef = userRef;
                        qAddPic.ModifierDate = DateTime.Now;
                        qAddPic.IsOk = true;
                        qAddPic.IsMain = true;

                        db.SaveChanges();

                    }


                }

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }

        public static List<VMBlogs.VMBlog> RepositoryMainBlogById(Guid id)
        {
            var list = new List<VMBlogs.VMBlog>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Blog.SingleOrDefault(c => c.Id == id);


                if (query != null)
                {
                    var vm = new VMBlogs.VMBlog()
                    {
                        Id = query.Id,
                        Code = query.Code,
                        IsOk = query.IsOk,
                        PrimaryTitle = query.PrimaryTitle,
                        SecondaryTitle = query.SecondaryTitle,
                        Sort = query.Sort,
                        Description = query.Description,
                        Summary = query.summary,
                        CreatorDate = query.CreatorDate,
                        Tag = query.Tag,
                        CategoryRef = query.CreatorRef,
                    };
                    var queryCreatorName = db.Table_User.FirstOrDefault(c => c.Id == query.CreatorRef);
                    if (queryCreatorName != null)
                    {
                        vm.CreatorName = queryCreatorName.Name;
                    }
                    var queryfile =
                        db.Table_File_Upload.SingleOrDefault(
                            c => c.IsOk && c.Ref == query.Id && c.IsMain);
                    if (queryfile != null)
                    {
                        vm.FileName = "/Static/Content/Images/BlogImages/" + queryfile.FileName +
                                      queryfile.FileExtensions;
                    }
                    else
                    {
                        vm.FileName = "/Static/Content/Images/";
                    }
                    var queryCategories =
                        db.Table_Blog_Category.FirstOrDefault(c => c.Id == query.CategoryRef && c.IsOk && !c.IsDelete);
                    if (queryCategories != null)
                    {
                        vm.CategoryTitle = queryCategories.PrimaryTitle;
                        vm.CategoryRef = queryCategories.Id;
                        vm.ParentRef = queryCategories.ParentRef;
                    }
                    list.Add(vm);

                }
            }
            catch (Exception e)
            {

            }

            return list;
        }

        public static List<VMBlogs.VMBlog> RepositorySearchWithPrimaryTitle(string SearchKey)
        {
            var list = new List<VMBlogs.VMBlog>();
            try
            {
                var db = new Orders_Entities();

                var queryBlog = db.Table_Blog.OrderByDescending(c => c.CreatorDate)
                    .Where(c => c.PrimaryTitle.Contains(SearchKey)).AsNoTracking().ToList();
                if (queryBlog.Count > 0)
                {
                    foreach (var product in queryBlog)
                    {
                        var vm = new VMBlogs.VMBlog()
                        {
                            Id = product.Id,
                            Code = product.Code,
                            IsOk = product.IsOk,
                            PrimaryTitle = product.PrimaryTitle,
                            SecondaryTitle = product.SecondaryTitle,
                            Sort = product.Sort,
                            Url = product.Url,
                            CreatorDate = product.CreatorDate,
                            Description = product.Description,
                            Summary = product.summary,
                        };

                        var queryfile =
                            db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == product.Id);
                        if (queryfile != null)
                        {
                            vm.FileName = "/Static/Content/Images/BlogImages/" + queryfile.FileName +
                                          queryfile.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Static/Content/Images/";
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

        public static List<VMBlogs.VMBlog> RepositoryMainBlogManagementById(Guid id)
        {
            var list = new List<VMBlogs.VMBlog>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Blog
                    .OrderByDescending(c => c.CreatorDate).Where(c => c.Id == id).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var blog in query)
                    {
                        var vm = new VMBlogs.VMBlog()
                        {
                            Id = blog.Id,
                            Code = blog.Code,
                            IsOk = blog.IsOk,
                            PrimaryTitle = blog.PrimaryTitle,
                            Url = blog.Url,
                            CreatorDate = blog.CreatorDate,
                            Description = blog.Description,
                            Summary = blog.summary,

                        };

                        var queryfile =
                            db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == blog.Id);
                        if (queryfile != null)
                        {
                            vm.FileName = "/Static/Content/Images/BlogImages/" + queryfile.FileName +
                                          queryfile.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Static/Content/Images/";
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


        public static List<VMBlogs.VMBlog> RepositoryMainBlogManagement()
        {
            var list = new List<VMBlogs.VMBlog>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Blog
                    .OrderByDescending(c => c.CreatorDate).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var blog in query)
                    {
                        var vm = new VMBlogs.VMBlog()
                        {
                            Id = blog.Id,
                            Code = blog.Code,
                            IsOk = blog.IsOk,
                            PrimaryTitle = blog.PrimaryTitle,
                            Url = blog.Url,
                            CreatorDate = blog.CreatorDate,
                            Description = blog.Description,
                            Summary = blog.summary,
                            Sort = blog.Sort,
                            Tag = blog.Tag,
                        };

                        var queryfile = db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == blog.Id);
                        if (queryfile != null)
                        {
                            vm.FileName = "/Static/Content/Images/BlogImages/" + queryfile.FileName +
                                          queryfile.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Static/Content/Images/";
                        }
                        var queryCategories =
                            db.Table_Blog_Category.FirstOrDefault(c => c.Id == blog.CategoryRef && c.IsOk && !c.IsDelete);
                        if (queryCategories != null)
                        {
                            vm.CategoryTitle = queryCategories.PrimaryTitle;
                            vm.CategoryRef = queryCategories.Id;
                            vm.ParentRef = queryCategories.ParentRef;
                        }

                        var queryUsers =
                            db.Table_User.FirstOrDefault(c => c.Id == blog.CreatorRef && c.IsOk && !c.IsDelete);
                        if (queryCategories != null)
                        {
                            vm.CreatorName = queryUsers.Name + " " + queryUsers.Family;
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


        public static List<VMBlogs.VMBlog> RepositoryBlogManagementSearch(string searchnew)
        {
            var list = new List<VMBlogs.VMBlog>();
            try
            {
                var search = searchnew.Replace("	", "");
                var searcha = string.IsNullOrWhiteSpace(searchnew);
                var db = new Orders_Entities();
                var query = db.Table_Blog
                    .OrderByDescending(c => c.CreatorDate).Where(c => c.Code == search || c.PrimaryTitle.Contains(search)).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var blog in query)
                    {
                        var vm = new VMBlogs.VMBlog()
                        {
                            Id = blog.Id,
                            Code = blog.Code,
                            IsOk = blog.IsOk,
                            PrimaryTitle = blog.PrimaryTitle,
                            Url = blog.Url,
                            CreatorDate = blog.CreatorDate,
                            Description = blog.Description,
                            Summary = blog.summary,
                            Sort = blog.Sort,
                        };

                        var queryfile =
                            db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == blog.Id);
                        if (queryfile != null)
                        {
                            vm.FileName = "/Static/Content/Images/BlogImages/" + queryfile.FileName +
                                          queryfile.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Static/Content/Images/";
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

        public static VMBlogs.VMBlog RepositoryMainBlogMangmentByid(Guid Id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Blog.FirstOrDefault(c => c.Id == Id);
                if (query != null)
                {
                    var vm = new VMBlogs.VMBlog()
                    {
                        Id = query.Id,
                        Code = query.Code,
                        IsOk = query.IsOk,
                        PrimaryTitle = query.PrimaryTitle,
                        SecondaryTitle = query.SecondaryTitle,
                        Url = query.Url,
                        CreatorDate = query.CreatorDate,
                        Sort = query.Sort,
                        Description = query.Description,
                        Summary = query.summary,
                        Tag = query.Tag
                    };

                    var queryfile =
                        db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == query.Id);
                    if (queryfile != null)
                    {
                        vm.FileName = "/Static/Content/Images/BlogImages/" + queryfile.FileName +
                                      queryfile.FileExtensions;
                    }
                    else
                    {
                        vm.FileName = "/Static/Content/Images/";
                    }


                    return vm;
                }
            }
            catch (Exception e)
            {

            }

            return new VMBlogs.VMBlog();
        }


        #endregion


    }
}