
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
    
public partial class Table_Links
{

    public System.Guid Id { get; set; }

    public string Code { get; set; }

    public string PrimaryTitle { get; set; }

    public string SecendryTitle { get; set; }

    public string TertiaryTitle { get; set; }

    public int Sort { get; set; }

    public bool IsOk { get; set; }

    public int Version { get; set; }

    public System.Guid CreatorRef { get; set; }

    public System.DateTime CreatorDate { get; set; }

    public System.Guid ModifierRef { get; set; }

    public System.DateTime ModifireDate { get; set; }

    public string Url { get; set; }

}

}
