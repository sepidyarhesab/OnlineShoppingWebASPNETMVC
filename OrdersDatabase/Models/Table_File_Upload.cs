
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
    
public partial class Table_File_Upload
{

    public System.Guid Id { get; set; }

    public string Code { get; set; }

    public System.Guid Ref { get; set; }

    public System.Guid LanguageRef { get; set; }

    public string Schemas { get; set; }

    public string Tables { get; set; }

    public string FileName { get; set; }

    public long FileLenght { get; set; }

    public string FileExtensions { get; set; }

    public string FileUniqeName { get; set; }

    public string Path { get; set; }

    public int Sort { get; set; }

    public bool IsOk { get; set; }

    public bool IsMain { get; set; }

    public bool IsDelete { get; set; }

    public System.Guid CreatorRef { get; set; }

    public System.DateTime CreatorDate { get; set; }

    public System.Guid ModifierRef { get; set; }

    public System.DateTime ModifierDate { get; set; }

    public int Version { get; set; }

}

}
