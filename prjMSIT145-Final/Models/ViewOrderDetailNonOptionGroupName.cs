using System;
using System.Collections.Generic;

namespace prjMSIT145_Final.Models
{
    public partial class ViewOrderDetailNonOptionGroupName
    {
        public int ItemFid { get; set; }
        public int? ProductFId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal? ProductUp { get; set; }
        public int? Qty { get; set; }
        public string? Options { get; set; }
        public decimal? OptionUp { get; set; }
    }
}
