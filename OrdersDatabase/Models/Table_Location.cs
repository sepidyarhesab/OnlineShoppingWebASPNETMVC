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
    
    public partial class Table_Location
    {
        public int LocationId { get; set; }
        public string Title { get; set; }
        public string Title_En { get; set; }
        public string Code { get; set; }
        public Nullable<int> Parent { get; set; }
        public string Type { get; set; }
        public int Version { get; set; }
        public Nullable<int> Creator { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<int> LastModifier { get; set; }
        public Nullable<System.DateTime> LastModificationDate { get; set; }
        public string MinistryofFinanceCode { get; set; }
        public string TaxFileCode { get; set; }
    }
}
