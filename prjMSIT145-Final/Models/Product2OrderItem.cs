using System;
using System.Collections.Generic;

namespace prjMSIT145_Final.Models
{
    public partial class Product2OrderItem
    {
        public int Fid { get; set; }
        public int? ItemFid { get; set; }
        public int? ProductFid { get; set; }
        public int? Qty { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
