using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersSettings.ViewModels.Settings
{
    
        public class ViewModelsMainViewConfigurations
        {
            public int Row { get; set; }
            public Guid Id { get; set; }
            public string Code { get; set; }
            public string PrimaryTitle { get; set; }
            public string SecondaryTitle { get; set; }
            public string TertiaryTitle { get; set; }
            public bool IsOk { get; set; }
            public string IsOkTitle { get; set; }
            public string IsOkClass { get; set; }
            public int Sort { get; set; }
            public string IsMainTitle { get; set; }
            public string IsMainClass { get; set; }
            public string View { get; set; }
        }
        


}
