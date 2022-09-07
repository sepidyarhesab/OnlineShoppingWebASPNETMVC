using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrdersOrders.ViewModels.Orders
{
    public class VMDiscount
    {

        public class VmDiscountManagement
        {
            public Guid Id { get; set; }
            public string Code { get; set; }
            public string DiscountCode { get; set; } 
            public string PrimaryTitle { get; set; }
            public string SecondaryTitle { get; set; }
            public string Entities { get; set; }
            public decimal? Discount { get; set; }
            public decimal? DiscountFee { get; set; }
            public decimal? DiscountPercent { get; set; }
            public int? DiscountUsed { get; set; }
            public int? DiscountCount { get; set; }
            public int? DiscountQuantity { get; set; }
            public Guid? DiscountUser { get; set; }
            public Guid? ProductRef { get; set; }
            public string ProductTitle { get; set; }
            public Guid? CategoriesRef { get; set; }
            public string CategoriesTitle { get; set; }
            public int Sort { get; set; }
            public int Row { get; set; }
            public bool IsOk { get; set; }
            public string IsOkTitle { get; set; }
            public string IsOkClass { get; set; }
            public Guid CreatorRef { get; set; }
            public DateTime CreatorDate { get; set; }
            public Guid ModifierRef { get; set; }
            public DateTime ModifierDate { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string StartDate1 { get; set; }
            public string EndDate1 { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public string StartTime1 { get; set; }
            public string EndTime1 { get; set; }
            public bool IsMain { get; set; }
            public string IsMainTitle { get; set; }
            public string IsMainClass { get; set; }
            public string FullName { get; set; }
        }
    }

}