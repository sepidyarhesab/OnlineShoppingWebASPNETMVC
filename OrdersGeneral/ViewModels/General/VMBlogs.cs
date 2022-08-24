using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OrdersInventory.ViewModels.Inventory;

namespace OrdersGeneral.ViewModels.General
{
    public class VMBlogs
    {
       
        public class VMBlog
        {
            public Guid Id { get; set; }
            public string Code { get; set; }
            public string PrimaryTitle { get; set; }
            public string SecondaryTitle { get; set; }
            public string Url { get; set; }
            public string FileName { get; set; }
            public bool IsOk { get; set; }
            public DateTime CreatorDate { get; set; }
            public List<VMProduct.FileUploadName> FileList { get; set; }
            public string Description { get; set; }
            public int? Sort { get; set; }
            public string Summary { get; set; }
            public  Guid? CategoryRef { get; set; }
            public string CategoryTitle { get; set; }
            public string CreatorName { get; set; }
            public Guid? ParentRef { get; set; }
            public string Tag { get; set; }
        }

        public class ViewModelCategories
        {
            public Guid Id { get; set; }
            public string PrimaryTitle { get; set; }
            public string SecondaryTitle { get; set; }
            public Guid? ParentRef { get; set; }
            public String Code { get; set; }
            public String Url { get; set; }
            public bool IsMain { get; set; }
            public bool IsDelete { get; set; }
            public bool IsOk { get; set; }
            public DateTime ModifierDate { get; set; }
            public DateTime CreatorDate { get; set; }
            public int Sort { get; set; }
            public List<ViewModelCategories> ViewModelSubCategoreis { get; set; }
        }

    }
}
