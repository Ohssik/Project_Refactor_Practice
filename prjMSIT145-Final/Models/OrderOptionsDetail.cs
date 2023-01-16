using System;
using System.Collections.Generic;

namespace prjMSIT145_Final.Models
{
    public partial class OrderOptionsDetail
    {
        public int Fid { get; set; }
        public int? ItemFid { get; set; }
        public int? OptionFid { get; set; }
    }
}
