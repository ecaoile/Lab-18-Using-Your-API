using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab18WebApp.Models
{
    public class Promo
    {
        public double OrderSubTotal { get; set; }
        public double PromoAmt { get; set; }
        public string PromoId { get; set; }
        public string PromotionName { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public double MinimumOrderValue { get; set; }
        public string PromotionType { get; set; }
    }
}
