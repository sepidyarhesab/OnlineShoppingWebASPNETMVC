using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersInventory.ViewModels.Inventory
{
    public class VMProductsSizeGuides
    {
        //ViewModel Table Product_SizeGuide
        public class ViewModelProductSizeGuide
        {
            public Guid Id { get; set; }
            public String Code { get; set; }
            public string PrimaryTitle { get; set; }
            public string SecondaryTitle { get; set; }
            public string CategoryTitle { get; set; }
            public Guid CategoriesRef { get; set; }
            public int Sort { get; set; }
            public bool IsOk { get; set; }
            public string IsOkClass { get; set; }
            public string IsOkTitle { get; set; }
            public bool IsMain { get; set; }
            public bool IsDelete { get; set; }
            public Guid CreatorRef { get; set; }
            public Guid? ModifierRef { get; set; }
            public DateTime CreatorDate { get; set; }
            public DateTime? ModifierDate { get; set; }
            public int Version { get; set; }
        }

        public class ViewModelProductSizeValuesGuide
        {
            public Guid Id { get; set; }
            public string Code { get; set; }
            public string PropertyTitle { get; set; }
            public string SecondaryTitle { get; set; }
            public string SizeGuideTitle { get; set; }
            public string SizeValue { get; set; }
            public Guid SizeGuideRef { get; set; }
            public int Sort { get; set; }
            public bool IsOk { get; set; }
            public string IsOkClass { get; set; }
            public string IsOkTitle { get; set; }
            public bool IsMain { get; set; }
            public bool IsDelete { get; set; }
            public Guid CreatorRef { get; set; }
            public Guid? ModifierRef { get; set; }
            public DateTime CreatorDate { get; set; }
            public DateTime? ModifierDate { get; set; }
            public int Version { get; set; }
        }
    }
}
