﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OrdersDatabase.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class Orders_Entities : DbContext
    {
        public Orders_Entities()
            : base("name=Orders_Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Table_Address> Table_Address { get; set; }
        public DbSet<Table_Banner> Table_Banner { get; set; }
        public DbSet<Table_Blog> Table_Blog { get; set; }
        public DbSet<Table_Blog_Category> Table_Blog_Category { get; set; }
        public DbSet<Table_Comments> Table_Comments { get; set; }
        public DbSet<Table_ContactUs> Table_ContactUs { get; set; }
        public DbSet<Table_Knowing> Table_Knowing { get; set; }
        public DbSet<Table_Links> Table_Links { get; set; }
        public DbSet<Table_Location> Table_Location { get; set; }
        public DbSet<Table_Menu> Table_Menu { get; set; }
        public DbSet<Table_MenuAccess> Table_MenuAccess { get; set; }
        public DbSet<Table_MenuAdmin> Table_MenuAdmin { get; set; }
        public DbSet<Table_MenuAdmin_Category> Table_MenuAdmin_Category { get; set; }
        public DbSet<Table_Role> Table_Role { get; set; }
        public DbSet<Table_Selection_Configuration> Table_Selection_Configuration { get; set; }
        public DbSet<Table_Service> Table_Service { get; set; }
        public DbSet<Table_Site_Type> Table_Site_Type { get; set; }
        public DbSet<Table_Slider> Table_Slider { get; set; }
        public DbSet<Table_SocialMedia> Table_SocialMedia { get; set; }
        public DbSet<Table_Static_SocialMedia> Table_Static_SocialMedia { get; set; }
        public DbSet<Table_User> Table_User { get; set; }
        public DbSet<Table_Whatsapp> Table_Whatsapp { get; set; }
        public DbSet<Table_Favorites> Table_Favorites { get; set; }
        public DbSet<Table_File_Upload> Table_File_Upload { get; set; }
        public DbSet<Table_Price_DisCount> Table_Price_DisCount { get; set; }
        public DbSet<Table_Product> Table_Product { get; set; }
        public DbSet<Table_Product_Category> Table_Product_Category { get; set; }
        public DbSet<Table_Product_Color> Table_Product_Color { get; set; }
        public DbSet<Table_Product_Property> Table_Product_Property { get; set; }
        public DbSet<Table_Product_PropertyList> Table_Product_PropertyList { get; set; }
        public DbSet<Table_Product_Selection> Table_Product_Selection { get; set; }
        public DbSet<Table_Product_Size> Table_Product_Size { get; set; }
        public DbSet<Table_Product_Summary> Table_Product_Summary { get; set; }
        public DbSet<Table_Stock> Table_Stock { get; set; }
        public DbSet<Table_Discount> Table_Discount { get; set; }
        public DbSet<Table_Order_Item> Table_Order_Item { get; set; }
        public DbSet<Table_Transfer> Table_Transfer { get; set; }
        public DbSet<Table_Meta_Configuration> Table_Meta_Configuration { get; set; }
        public DbSet<Table_Settings> Table_Settings { get; set; }
        public DbSet<Table_SMS_Log> Table_SMS_Log { get; set; }
        public DbSet<Table_View_Configuration> Table_View_Configuration { get; set; }
        public DbSet<Table_Order> Table_Order { get; set; }
    
        public virtual int SP_InsertOrder(Nullable<System.Guid> id, string code, string firstname, string lastname, string phone, string address, string productCode, string postalCode, Nullable<byte> status, string transactionCode, string deliveryCode, string note, string quantity, Nullable<bool> isPay, Nullable<decimal> discount, Nullable<decimal> tax, Nullable<decimal> duty, Nullable<decimal> addition, Nullable<decimal> price, Nullable<bool> isOk, Nullable<System.Guid> modifierRef, Nullable<System.DateTime> modifierDate, Nullable<System.Guid> creatorRef, Nullable<System.DateTime> creatorDate, Nullable<int> version)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(System.Guid));
    
            var codeParameter = code != null ?
                new ObjectParameter("Code", code) :
                new ObjectParameter("Code", typeof(string));
    
            var firstnameParameter = firstname != null ?
                new ObjectParameter("Firstname", firstname) :
                new ObjectParameter("Firstname", typeof(string));
    
            var lastnameParameter = lastname != null ?
                new ObjectParameter("Lastname", lastname) :
                new ObjectParameter("Lastname", typeof(string));
    
            var phoneParameter = phone != null ?
                new ObjectParameter("Phone", phone) :
                new ObjectParameter("Phone", typeof(string));
    
            var addressParameter = address != null ?
                new ObjectParameter("Address", address) :
                new ObjectParameter("Address", typeof(string));
    
            var productCodeParameter = productCode != null ?
                new ObjectParameter("ProductCode", productCode) :
                new ObjectParameter("ProductCode", typeof(string));
    
            var postalCodeParameter = postalCode != null ?
                new ObjectParameter("PostalCode", postalCode) :
                new ObjectParameter("PostalCode", typeof(string));
    
            var statusParameter = status.HasValue ?
                new ObjectParameter("Status", status) :
                new ObjectParameter("Status", typeof(byte));
    
            var transactionCodeParameter = transactionCode != null ?
                new ObjectParameter("TransactionCode", transactionCode) :
                new ObjectParameter("TransactionCode", typeof(string));
    
            var deliveryCodeParameter = deliveryCode != null ?
                new ObjectParameter("DeliveryCode", deliveryCode) :
                new ObjectParameter("DeliveryCode", typeof(string));
    
            var noteParameter = note != null ?
                new ObjectParameter("Note", note) :
                new ObjectParameter("Note", typeof(string));
    
            var quantityParameter = quantity != null ?
                new ObjectParameter("Quantity", quantity) :
                new ObjectParameter("Quantity", typeof(string));
    
            var isPayParameter = isPay.HasValue ?
                new ObjectParameter("IsPay", isPay) :
                new ObjectParameter("IsPay", typeof(bool));
    
            var discountParameter = discount.HasValue ?
                new ObjectParameter("Discount", discount) :
                new ObjectParameter("Discount", typeof(decimal));
    
            var taxParameter = tax.HasValue ?
                new ObjectParameter("Tax", tax) :
                new ObjectParameter("Tax", typeof(decimal));
    
            var dutyParameter = duty.HasValue ?
                new ObjectParameter("Duty", duty) :
                new ObjectParameter("Duty", typeof(decimal));
    
            var additionParameter = addition.HasValue ?
                new ObjectParameter("Addition", addition) :
                new ObjectParameter("Addition", typeof(decimal));
    
            var priceParameter = price.HasValue ?
                new ObjectParameter("Price", price) :
                new ObjectParameter("Price", typeof(decimal));
    
            var isOkParameter = isOk.HasValue ?
                new ObjectParameter("IsOk", isOk) :
                new ObjectParameter("IsOk", typeof(bool));
    
            var modifierRefParameter = modifierRef.HasValue ?
                new ObjectParameter("ModifierRef", modifierRef) :
                new ObjectParameter("ModifierRef", typeof(System.Guid));
    
            var modifierDateParameter = modifierDate.HasValue ?
                new ObjectParameter("ModifierDate", modifierDate) :
                new ObjectParameter("ModifierDate", typeof(System.DateTime));
    
            var creatorRefParameter = creatorRef.HasValue ?
                new ObjectParameter("CreatorRef", creatorRef) :
                new ObjectParameter("CreatorRef", typeof(System.Guid));
    
            var creatorDateParameter = creatorDate.HasValue ?
                new ObjectParameter("CreatorDate", creatorDate) :
                new ObjectParameter("CreatorDate", typeof(System.DateTime));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("Version", version) :
                new ObjectParameter("Version", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_InsertOrder", idParameter, codeParameter, firstnameParameter, lastnameParameter, phoneParameter, addressParameter, productCodeParameter, postalCodeParameter, statusParameter, transactionCodeParameter, deliveryCodeParameter, noteParameter, quantityParameter, isPayParameter, discountParameter, taxParameter, dutyParameter, additionParameter, priceParameter, isOkParameter, modifierRefParameter, modifierDateParameter, creatorRefParameter, creatorDateParameter, versionParameter);
        }
    
        public virtual int SP_InsertOrderItem(Nullable<System.Guid> id, string code, Nullable<System.Guid> orderRef, string itemCode, Nullable<decimal> quantity, Nullable<decimal> fee, Nullable<System.Guid> sizeRef, Nullable<System.Guid> colorRef, Nullable<System.Guid> itemRef, Nullable<decimal> discount, Nullable<decimal> addition, Nullable<decimal> tax, Nullable<decimal> duty, Nullable<decimal> price)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(System.Guid));
    
            var codeParameter = code != null ?
                new ObjectParameter("Code", code) :
                new ObjectParameter("Code", typeof(string));
    
            var orderRefParameter = orderRef.HasValue ?
                new ObjectParameter("OrderRef", orderRef) :
                new ObjectParameter("OrderRef", typeof(System.Guid));
    
            var itemCodeParameter = itemCode != null ?
                new ObjectParameter("ItemCode", itemCode) :
                new ObjectParameter("ItemCode", typeof(string));
    
            var quantityParameter = quantity.HasValue ?
                new ObjectParameter("Quantity", quantity) :
                new ObjectParameter("Quantity", typeof(decimal));
    
            var feeParameter = fee.HasValue ?
                new ObjectParameter("Fee", fee) :
                new ObjectParameter("Fee", typeof(decimal));
    
            var sizeRefParameter = sizeRef.HasValue ?
                new ObjectParameter("SizeRef", sizeRef) :
                new ObjectParameter("SizeRef", typeof(System.Guid));
    
            var colorRefParameter = colorRef.HasValue ?
                new ObjectParameter("ColorRef", colorRef) :
                new ObjectParameter("ColorRef", typeof(System.Guid));
    
            var itemRefParameter = itemRef.HasValue ?
                new ObjectParameter("ItemRef", itemRef) :
                new ObjectParameter("ItemRef", typeof(System.Guid));
    
            var discountParameter = discount.HasValue ?
                new ObjectParameter("Discount", discount) :
                new ObjectParameter("Discount", typeof(decimal));
    
            var additionParameter = addition.HasValue ?
                new ObjectParameter("Addition", addition) :
                new ObjectParameter("Addition", typeof(decimal));
    
            var taxParameter = tax.HasValue ?
                new ObjectParameter("Tax", tax) :
                new ObjectParameter("Tax", typeof(decimal));
    
            var dutyParameter = duty.HasValue ?
                new ObjectParameter("Duty", duty) :
                new ObjectParameter("Duty", typeof(decimal));
    
            var priceParameter = price.HasValue ?
                new ObjectParameter("Price", price) :
                new ObjectParameter("Price", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_InsertOrderItem", idParameter, codeParameter, orderRefParameter, itemCodeParameter, quantityParameter, feeParameter, sizeRefParameter, colorRefParameter, itemRefParameter, discountParameter, additionParameter, taxParameter, dutyParameter, priceParameter);
        }
    
        public virtual int SP_UpdateOrder(Nullable<System.Guid> id, string code, Nullable<bool> isPay, Nullable<bool> isOk)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(System.Guid));
    
            var codeParameter = code != null ?
                new ObjectParameter("Code", code) :
                new ObjectParameter("Code", typeof(string));
    
            var isPayParameter = isPay.HasValue ?
                new ObjectParameter("IsPay", isPay) :
                new ObjectParameter("IsPay", typeof(bool));
    
            var isOkParameter = isOk.HasValue ?
                new ObjectParameter("IsOk", isOk) :
                new ObjectParameter("IsOk", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_UpdateOrder", idParameter, codeParameter, isPayParameter, isOkParameter);
        }
    
        public virtual int SP_InsertSMS(string code, string messages, Nullable<System.Guid> userRef)
        {
            var codeParameter = code != null ?
                new ObjectParameter("Code", code) :
                new ObjectParameter("Code", typeof(string));
    
            var messagesParameter = messages != null ?
                new ObjectParameter("Messages", messages) :
                new ObjectParameter("Messages", typeof(string));
    
            var userRefParameter = userRef.HasValue ?
                new ObjectParameter("UserRef", userRef) :
                new ObjectParameter("UserRef", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_InsertSMS", codeParameter, messagesParameter, userRefParameter);
        }
    }
}