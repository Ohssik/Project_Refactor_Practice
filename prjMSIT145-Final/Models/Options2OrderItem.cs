using System;
using System.Collections.Generic;

namespace prjMSIT145_Final.Models
{
    public partial class Options2OrderItem
    {
        public int Fid { get; set; }
        public int? ItemFid { get; set; }
        public int? OpGroupFid { get; set; }
        public int? OptionFid { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
