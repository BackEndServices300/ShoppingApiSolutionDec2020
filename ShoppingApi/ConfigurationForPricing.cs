using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi
{
    public class ConfigurationForPricing
    {
        public string SectionName = "productPricing";
        public decimal Markup { get; set; }
    }
}
