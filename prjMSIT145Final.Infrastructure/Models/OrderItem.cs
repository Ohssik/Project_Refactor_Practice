using System;
using System.Collections.Generic;

namespace prjMSIT145Final.Infrastructure.Models
{
    public partial class OrderItem
    {
        public int Fid { get; set; }
        public int? OrderFid { get; set; }
        public int? ProductFid { get; set; }
        public int? Qty { get; set; }
    }
}
