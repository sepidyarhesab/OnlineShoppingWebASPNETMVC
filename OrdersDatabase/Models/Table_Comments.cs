
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
    
public partial class Table_Comments
{

    public System.Guid Id { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public byte Rating { get; set; }

    public System.Guid ProductRef { get; set; }

    public string Phone { get; set; }

    public string Note { get; set; }

    public long Likes { get; set; }

    public long Dislikes { get; set; }

    public bool IsOk { get; set; }

    public bool IsDelete { get; set; }

    public System.Guid CreatorRef { get; set; }

    public System.DateTime CreatorDate { get; set; }

    public System.Guid ModifierRef { get; set; }

    public System.DateTime ModifierDate { get; set; }

    public int Version { get; set; }

    public Nullable<System.Guid> ParentRef { get; set; }

}

}
