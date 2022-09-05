using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrdersOrders.ViewModels.Orders
{
    public class VMOrders
    {
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






        public class VmOrderSubmit
        {
            public string Name { get; set; }
            public string FileName { get; set; }
            public string ProductTitle { get; set; }
            public string Family { get; set; }
            public string Phone { get; set; }
            public string PageId { get; set; }
            public string Code { get; set; }
            public decimal Quantity { get; set; }
            public decimal Fee { get; set; }
            public string Note { get; set; }
            public string Address { get; set; }
            public string Size { get; set; }
            public Guid SizeRef { get; set; }
            public Guid ColorRef { get; set; }
            public Guid ProductId { get; set; }
            public Guid OrderRef { get; set; }
            public string SizeTitle { get; set; }
            public string ColorTitle { get; set; }
            public string Color { get; set; }
            public decimal? Discount { get; set; }
            public string DiscountCode { get; set; }
            public bool IsOk { get; set; }
            public decimal Price { get; set; }
            public Guid? CategoryRef { get; set; }
            public string Message { get; set; }
            public string DisCode { get; set; }
            public bool DisUse { get; set; }
            public decimal? SumDiscount { get; set; }
            public decimal? RowDiscount { get; set; }
        }


        public class VmOrderCarts
        {
            public List<VmOrderSubmit> CartsItems { get; set; }
            public decimal Sum { get; set; }
            public decimal Discount { get; set; }
            public decimal DiscountFree { get; set; }
            public decimal DiscountPercent { get; set; }
            public decimal Transfer { get; set; }
            public decimal SumPay { get; set; }
            //public decimal sumdis { get; set; }
            public decimal TransferDis { get; set; }
        }


        public class VmOrderItemSubmit
        {

            public string ItemCode { get; set; }
            public decimal Quantity { get; set; }
            public decimal Fee { get; set; }
            public decimal NetFee { get; set; }

        }

        public class VmOrderMangment
        {
            public Guid Id { get; set; }
            public Guid UserRef { get; set; }
            public int CityReff { get; set; }
            public int CityRef { get; set; }
            public string CityTitle { get; set; }
            public string StateTitle { get; set; }
            public string Name { get; set; }
            public DateTime time { get; set; }
            public string Family { get; set; }
            public string Fullname { get; set; }
            public string Phone { get; set; }
            public string PageId { get; set; }
            public bool? Sex { get; set; }
            public string Code { get; set; }
            public decimal Quantity { get; set; }
            public string Note { get; set; }
            public string Address { get; set; }
            public DateTime CreatorDateTime { get; set; }
            public DateTime ModifierDateTime { get; set; }
            public bool IsMain { get; set; }
            public bool IsOk { get; set; }
            public bool IsPay { get; set; }
            public string IsPayTitle { get; set; }
            public string IsPayClass { get; set; }
            public string Status { get; set; }
            public decimal Price { get; set; }
            public string Password { get; set; }
            public string PostalCode { get; set; }
            public string ProductCode { get; set; }
            public string DeliveryCode { get; set; }
            public string TransactionCode { get; set; }
            public string Identification { get; set; }
            public byte Case { get; set; }
            public decimal Discount { get; set; }
            public int Type { get; set; }
            public string Typee { get; set; }
            public List<VmOrderMangment> AddresList { get; set; }
        }



        public class VmOrderItem
        {
            public Guid Id { get; set; }
            public Guid ProductId { get; set; }
            public Guid OrderRef { get; set; }
            public decimal Fee { get; set; }
            public decimal Quantity { get; set; }
            public decimal SumRow { get; set; }
            public string Title { get; set; }
            public string SecTitle { get; set; }
            public string Size { get; set; }
            public string SizeTitle { get; set; }
            public string Code { get; set; }
            public string Datetime { get; set; }
            public Guid? SizeRef { get; set; }
            public Guid? ColorRef { get; set; }
            public Guid? CatRef { get; set; }
            public string ColorTitle { get; set; }
            public string Color { get; set; }
            public string FileName { get; set; }
            public string Name { get; set; }
            public string Family { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }
            public string Note { get; set; }
            public string PostalCode { get; set; }
            public string TransactionCode { get; set; }
            public string DeliveryCode { get; set; }
            public string OrderCode { get; set; }
            public string CityTitle { get; set; }
            public int Discount { get; set; }
            public decimal Discounts { get; set; }
            public decimal Price { get; set; }
            public decimal ProductDiscount { get; set; }
            public decimal ProductPrice { get; set; }
        }
    }
}