using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OrdersDatabase.Models;
using OrdersSettings.ViewModels;


namespace OrdersSettings.Repository.Settings
{
    public  static  class Repfooter
    {
        public static List<Vmsettinginformation.Vmsettingfooter>RepInformationFooter()
        {
            var list = new List<Vmsettinginformation.Vmsettingfooter>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Settings.AsNoTracking().FirstOrDefault();
                if (query != null)
                {
                    var vm = new Vmsettinginformation.Vmsettingfooter
                    {
                        PrimaryTitle = query.PrimaryTitle,
                        Phone = query.Phone,
                        Id = query.Id,
                        Aparat = query.Aparat,
                        Email = query.Email,
                        Facebook = query.Facebook,
                        Instagram = query.Instagram,
                        Telegram = query.Telegram,
                        Whatsapp = query.Whatsapp,
                        Note = query.Note,
                        Twitter = query.Twitter,
                        Address = query.TertiaryTitle,
                        TarnsferPay = query.SeconderyTitle,
                    };

                    
                    var queryFile =
                        db.Table_File_Upload.FirstOrDefault(c => c.Ref == query.Id && c.IsMain && c.IsOk);
                    if (queryFile != null)
                    {
                        vm.FileName = "/Static/Content/Images/Logo/" + queryFile.FileUniqeName;
                    }
                    else
                    {
                        vm.FileName = "/Static/Content/Images/Logo/Logo.png";
                    }

                    var queryFile2 =
                        db.Table_File_Upload.FirstOrDefault(c => c.Ref == query.Id && !c.IsMain && c.IsOk);
                    if (queryFile2 != null)
                    {
                        vm.FileName2 = "/Static/Content/Images/Logo/" + queryFile2.FileUniqeName;
                    }
                    else
                    {
                        vm.FileName2 = "/Static/Content/Images/Logo/Logo.png";
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
    }
}