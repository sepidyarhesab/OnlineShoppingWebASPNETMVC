
//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
public partial class Table_Discount
{

    public System.Guid Id { get; set; }

    public string Code { get; set; }

    public string DiscountCode { get; set; }

    public Nullable<decimal> Discount { get; set; }

    public Nullable<decimal> DiscountFee { get; set; }

    public Nullable<decimal> DiscountPercent { get; set; }

    public Nullable<int> DiscountUsed { get; set; }

    public Nullable<int> DiscountCount { get; set; }

    public Nullable<System.Guid> DiscountUser { get; set; }

    public Nullable<int> DiscountQuantity { get; set; }

    public string Entities { get; set; }

    public System.DateTime StartDate { get; set; }

    public System.DateTime EndDate { get; set; }

    public Nullable<System.DateTime> StartTime { get; set; }

    public Nullable<System.DateTime> EndTime { get; set; }

    public Nullable<System.Guid> ProductRef { get; set; }

    public Nullable<System.Guid> CategoriesRef { get; set; }

    public int Sort { get; set; }

    public bool IsOk { get; set; }

    public bool IsMain { get; set; }

    public bool IsDelete { get; set; }

    public int Version { get; set; }

    public System.Guid CreatorRef { get; set; }

    public System.DateTime CreatorDate { get; set; }

    public System.Guid ModifierRef { get; set; }

    public System.DateTime ModifireDate { get; set; }

}

}