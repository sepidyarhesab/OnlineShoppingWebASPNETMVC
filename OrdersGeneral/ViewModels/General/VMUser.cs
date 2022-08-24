using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrdersGeneral.ViewModels.General
{
    public class VMUser
    {
        public class VMRole
        {
            public Guid Id { get; set; }
            public string PrimaryTitle { get; set; }
            public string SecondaryTitle { get; set; }
            public Guid? ParentRef { get; set; }
            public String Code { get; set; }
            public bool IsMain { get; set; }
            public bool IsDelete { get; set; }
            public bool IsOk { get; set; }
            public DateTime ModifierDate { get; set; }
            public DateTime CreatorDate { get; set; }
            public int Sort { get; set; }

        }
        public class VMUsers
        {
            public Guid Id { get; set; }
            public string Code { get; set; }
            public string IdentificationCode { get; set; }
            public string Name { get; set; }
            public string Family { get; set; }
            public string Url { get; set; }
            public bool? IsOk { get; set; }
            public DateTime CreatorDate { get; set; }
            public bool IsInBlacklist {get; set; }
            public bool IsDelete {get; set; }
            public bool Sex {get; set; }
            public string Phone { get; set; }
            public string Password { get; set; }
            public  Guid RoleRef { get; set; }
            public string RoleTitle { get; set; }
            public string FullName { get; set; }
           
        }
    }
}