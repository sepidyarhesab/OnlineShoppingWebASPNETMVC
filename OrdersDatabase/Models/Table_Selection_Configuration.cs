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
    
    public partial class Table_Selection_Configuration
    {
        public System.Guid Id { get; set; }
        public string Code { get; set; }
        public string PrimaryTitle { get; set; }
        public string SecondaryTitle { get; set; }
        public string tertiaryTitle { get; set; }
        public string Url { get; set; }
        public bool IsOk { get; set; }
        public bool IsDelete { get; set; }
        public System.Guid CreatorRef { get; set; }
        public System.DateTime CreatorDate { get; set; }
        public Nullable<System.Guid> ModifierRef { get; set; }
        public Nullable<System.DateTime> ModifierDate { get; set; }
        public int Version { get; set; }
        public Nullable<System.Guid> CategoriesRef { get; set; }
        public int Sort { get; set; }
        public int Count { get; set; }
    }
}
