using System;
using System.Collections.Generic;

namespace prjMSIT145Final.Infrastructure.Models
{
    public partial class ViewOrderDetail
    {
        public int? OrderFid { get; set; }
        public int ItemFid { get; set; }
        public int? ProductFid { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal? PUnitPrice { get; set; }
        public int? Qty { get; set; }
        public int? OpGroupFid { get; set; }
        public string? OptionGroupName { get; set; }
        public int? OptionFid { get; set; }
        public string? OptionName { get; set; }
        public decimal? OUp { get; set; }
    }
}
