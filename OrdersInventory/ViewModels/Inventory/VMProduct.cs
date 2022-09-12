using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace OrdersInventory.ViewModels.Inventory
{
    public class VMProduct
    {
        public class VmMainProduct
        {
            public Guid Id { get; set; }
            public string Code { get; set; }
            public string PrimaryTitle { get; set; }
            public string SecondaryTitle { get; set; }
            public string TertiaryTitle { get; set; }
            public string Url { get; set; }
            public string FileName { get; set; }
            public decimal Fee { get; set; }
            public string QuantityTitle { get; set; }
            public string QuantityClass { get; set; }
            public bool IsOk { get; set; }
            public decimal Quantity { get; set; }
            public DateTime CreatorDateTime { get; set; }
            public decimal Sort { get; set; }
            public decimal Discount { get; set; }
            public decimal ProductDiscount { get; set; }
            public string Note { get; set; }
            public List<FileUploadName> FileList { get; set; }
            public List<ProductProperty> PropertyList { get; set; }
            public Guid? CategoriesRef { get; set; }
            public string CategoreisTitle { get; set; }
            public string ColorCode { get; set; }
            public Guid? ColorId { get; set; } 
            public Guid? SizeId { get; set; }
    }

        public class FileUploadName
        {
            public string FileName { get; set; }
        }  
        public class ProductProperty
        {
            public string Id { get; set; }
            public string Key { get; set; }
            public string Value { get; set; }
            public bool IsOk { get; set; }
            public bool IsMain { get; set; }
            public string IsOkTitle { get; set; }
            public string IsOkClass { get; set; }
            public string IsMainTitle { get; set; }
            public string IsMainClass { get; set; }
        }
        public class VmMainProductMangement
        {
            public Guid Id { get; set; }
            public Guid? CategoriesRef { get; set; }
            public Guid ColorRef { get; set; }
            public string ColorTitle { get; set; }
            public Guid SizeRef { get; set; }
            public string SizeTitle { get; set; }
            public Guid ProductRef { get; set; }
            public Guid? Ref { get; set; }
            public string Code { get; set; }
            public string PrimaryTitle { get; set; }
            public string SecondaryTitle { get; set; }
            public string TertiaryTitle { get; set; }
            public string Url { get; set; }
            public string FileName { get; set; }
            public decimal Fee { get; set; }
            public string QuantityTitle { get; set; }
            public string QuantityClass { get; set; }
            public DateTime CreatorDateTime { get; set; }
            public DateTime ModifierDateTime { get; set; }
            public bool IsOk { get; set; }
            public string IsOkTitle { get; set; }
            public string IsOkClass { get; set; }   
            public string IsMainTitle { get; set; }
            public string IsMainClass { get; set; }
            public decimal Quantity { get; set; }
            public int Sort { get; set; }
            public string Note { get; set; }
            public string CategoreisTitle { get; set; }
            public decimal Discount { get; set; }
            public decimal Row { get; set; }
        }

        public class VmMainProductMangementExport
        {
            public string CodeProduct { get; set; }
            public string CodeSummary { get; set; }
            public string PrimaryTitle { get; set; }
            public string Size { get; set; }
            public Guid SizeRef { get; set; }
            public string Color { get; set; }
            public Guid ColorRef { get; set; }
            public Guid ProductRef { get; set; }
            public decimal Fee { get; set; }
            public decimal Quantity { get; set; }
        }
        public class ViewModelCategoreis
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
            public string FileName { get; set; }
            public string IsOkTitle { get; set; }
            public string IsOkClass { get; set; }

            public List<ViewModelCategoreis> ViewModelSubCategoreis { get; set; }
        }

        public class ViewModelConfiguration
        {
            public Guid Id { get; set; }
            public string PrimaryTitle { get; set; }
            public string Url { get; set; }
            public string SecondaryTitle { get; set; }
            public Guid? CategoriesRef { get; set; }
            public String Code { get; set; }
            public bool IsDelete { get; set; }
            public Guid CreatorRef { get; set; }
            public Guid? ModifierRef { get; set; }
            public DateTime CreatorDate { get; set; }
            public DateTime? ModifierDate { get; set; }
            public bool IsOk { get; set; }
            public int Sort { get; set; }
            public int Count { get; set; }
            public string CategoryTitle { get; set; }


        }


        public class ViewModelProductSize
        {
            public int Version { get; set; }
            public Guid Id { get; set; }
            public string PrimaryTitle { get; set; }
            public string SecondaryTitle { get; set; }
            public String Code { get; set; }
            public bool IsDelete { get; set; }
            public bool IsOk { get; set; }
            public Guid CreatorRef { get; set; }
            public Guid ModifierRef { get; set; }
            public DateTime CreatorDate { get; set; }
            public DateTime ModifierDate { get; set; }
            public string SizeTitle { get; set; }
            public int Sort { get; set; }
        }
        

        public class ViewModelProductColor
        {
            public int Version { get; set; }
            public Guid Id { get; set; }
            public string PrimaryTitle { get; set; }
            public string SecondaryTitle { get; set; }
            public Guid ColorRef { get; set; }
            public String Code { get; set; }
            public bool IsDelete { get; set; }
            public bool IsOk { get; set; }
            public Guid CreatorRef { get; set; }
            public DateTime CreatorDate { get; set; }
            public string ColorCode { get; set; }
        }

        public class ViewModelProductPropertyList
        {
            public int Version { get; set; }
            public Guid Id { get; set; }
            public string PrimaryTitle { get; set; }
            public string ProductTitle { get; set; }
            public string ColorTitle { get; set; }
            public string ColorCode { get; set; }
            public string SizeTitle { get; set; }
            public string SecondaryTitle { get; set; }
            public String Code { get; set; }
            public bool IsDelete { get; set; }
            public bool IsOk { get; set; }
            public string IsOkTitle { get; set; }
            public string IsOkClass { get; set; }
            public Guid CreatorRef { get; set; }
            public DateTime CreatorDate { get; set; }
            public Guid? ColorRef { get; set; }
            public Guid? SizeRef { get; set; }
            public Guid ProductRef { get; set; }
            public int Count { get; set; }
            public int Sort { get; set; }
            public bool IsMain { get; set; }
            public string IsMainTitle { get; set; }
            public string IsMainClass { get; set; }
            public decimal Quantity { get; set; }
            public decimal Fee { get; set; }
        }
        
        public class ViewModelProductSelection
        {
            public Guid Id { get; set; }
            public string PrimaryTitle { get; set; }
            public string SecondaryTitle { get; set;}
            public Guid ProductRef { get; set; }
            public Guid ConfigurationRef { get; set; }
            public String Code { get; set; }
            public bool IsDelete { get; set; }
            public bool IsOk { get; set; }
            public Guid CreatorRef { get; set; }
            public DateTime CreatorDate { get; set; }
            public int Sort { get; set; }
            public int Count { get; set; }
            public List<ViewModelConfiguration> ConfigurationList { get; set; }
            public List<VmMainProduct> ProductList { get; set; }

        }




        public class ViewModelDiscount
        {
            public string DiscountCode { get; set; }
            public int DiscountPercent { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public int UsableCount { get; set; }
            public Guid Id { get; set; }
            public string Code { get; set; }
            public DateTime CreatorDateTime { get; set; }
            public DateTime ModifierDateTime { get; set; }
            public bool IsOk { get; set; }
            public bool IsDelete { get; set; }
            public Guid CreatorRef { get; set; }
            public Guid? ModifierRef { get; set; }
        }







        public class ViewModelProductDiscount
        {
            public Guid Id { get; set; }
            public string DiscountCode { get; set; }
            public string PrimaryTitle { get; set; }
            public string SecondaryTitle { get; set; }
            public Guid? CategoriesRef { get; set; }
            public String Code { get; set; }
            public bool IsDelete { get; set; }
            public Guid CreatorRef { get; set; }
            public Guid? ModifierRef { get; set; }
            public DateTime CreatorDate { get; set; }
            public DateTime? ModifierDate { get; set; }
            public bool IsOk { get; set; }
            public decimal Discount { get; set; }
            public int? Count { get; set; }
            public int Sort { get; set; }
            public string CategoryTitle { get; set; }
        }


        public class ViewModelAjaxProduct
        {
            public decimal Discount { get; set; }
            public decimal Percent { get; set; }
            public decimal Fee { get; set; }
            public decimal Price { get; set; }
            public decimal Quantity { get; set; }
        }



    }
}