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
    public static class RepWhatsApp
    {
        public static List<VmWhatsApp.VmWhatsAppManagement> RepositoryWhatsAppManagement()
        {
            var list = new List<VmWhatsApp.VmWhatsAppManagement>();
            try
            {
                var db = new Orders_Entities();
                var rol = "5bf16139-6715-4190-988a-4849c1a0241d";
                var query = db.Table_Whatsapp.Where(c => !c.IsDelete).ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VmWhatsApp.VmWhatsAppManagement
                        {
                            Id = item.Id,
                            Code = item.Code,
                            PrimaryTitle = item.PrimaryTitle,
                            SecondaryTitle = item.SecondaryTitle,
                            IsDelete = item.IsDelete,
                            IsOk = item.IsOk,
                            CreatorDate = item.CreatorDate,
                            CreatorRef = item.CreatorRef,
                           
                        };


                        var queryIconPic = db.Table_File_Upload.FirstOrDefault(c =>
                            c.Ref == item.Id && c.IsOk && !c.IsDelete);
                        if (queryIconPic != null)
                        {
                            vm.FileName = "/Static/Content/Images/WhatsApp/" + queryIconPic.FileName +
                                          queryIconPic.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Static/Content/Images/WhatsApp/Logo_SP-637830308030933133.png";
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

                return list;
            }
            catch (Exception e)
            {
                return list;
            }
        }

        public static string ChangeStatus(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Whatsapp.FirstOrDefault(c => c.Id == id);
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

        public static string Delete(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Whatsapp.FirstOrDefault(c => c.Id == id);
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
                                        if (File.Exists(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadKnowing + fileUpload.FileName + fileUpload.FileExtensions)))
                                        {
                                            File.Delete(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadWhatsApp + fileUpload.FileName + fileUpload.FileExtensions));
                                        }
                                        if (File.Exists(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadWhatsApp + fileUpload.FileName + fileUpload.FileExtensions)))
                                        {
                                            File.Delete(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadWhatsApp + fileUpload.FileName + fileUpload.FileExtensions));
                                        }
                                        db.Table_File_Upload.Remove(fileUpload);
                                        db.SaveChanges();
                                    }
                                }
                                db.Table_Whatsapp.Remove(query);
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

        public static VmWhatsApp.VmWhatsAppManagement Edit(Guid? Id)
        {
            var db = new Orders_Entities();
            var query = db.Table_Whatsapp.FirstOrDefault(c => c.Id == Id);
            if (query != null)
            {
                var vm = new VmWhatsApp.VmWhatsAppManagement
                {
                    Id = query.Id,
                    PrimaryTitle = query.PrimaryTitle,
                    Code = query.Code,
                    SecondaryTitle = query.SecondaryTitle,
                    
                };
                return vm;
            }
            else
            {
                return new VmWhatsApp.VmWhatsAppManagement();
            }
        }

        public static string Add(VmWhatsApp.VmWhatsAppManagement value, Guid UserId, HttpPostedFileBase FileName)
        {
            try
            {
                var random = new Random();
                var db = new Orders_Entities();
                var UserRef = UserId;
                var id = Guid.NewGuid();
                var code = SepidyarHesabCodeGenerator.GenerateCode("", "");
                var query = db.Table_Whatsapp.Add(new Table_Whatsapp()
                {
                    Id = id,
                    PrimaryTitle = value.PrimaryTitle,
                    SecondaryTitle = value.SecondaryTitle,
                    Code = code,
                    IsDelete = value.IsDelete,
                    CreatorDate = DateTime.Now,
                    ModifireDate = DateTime.Now,
                    Version = 1,
                    CreatorRef = UserRef,
                    ModifierRef = UserRef,
                    IsOk = true,
                    Phone = "989915490757",
                });
                db.Table_Whatsapp.Add(query);
                db.SaveChanges();
                
                var filename = "Default";
                var fileExtention = "png";
                var filelenght = 200;
                if (FileName != null)
                {

                    filelenght = FileName.ContentLength;
                    filename = "Menu_" + code;
                    fileExtention = Path.GetExtension(FileName.FileName);
                    string pathCombine =
                        HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadWhatsApp + filename + fileExtention);
                    FileName.SaveAs(pathCombine);


                    var qAddPic = db.Table_File_Upload.Add(new Table_File_Upload());
                    qAddPic.Id = Guid.NewGuid();
                    qAddPic.Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_File_Upload");
                    qAddPic.Tables = "Table_Menu_Admin";
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
                    qAddPic.CreatorRef = UserId;
                    qAddPic.ModifierRef = UserId;
                    qAddPic.ModifierDate = DateTime.Now;
                    qAddPic.IsOk = true;
                    qAddPic.IsMain = true;
                    db.SaveChanges();

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
