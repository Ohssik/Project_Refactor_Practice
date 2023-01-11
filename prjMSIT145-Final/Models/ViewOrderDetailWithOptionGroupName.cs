using System;
using System.Collections.Generic;

namespace prjMSIT145_Final.Models
{
    public partial class ViewOrderDetailWithOptionGroupName
    {
        public int? OrderFid { get; set; }
        public string? ItemId { get; set; }
        public int? ProductFid { get; set; }
        public string? ProductName { get; set; }
        public decimal? PUnitPrice { get; set; }
        public int? Qty { get; set; }
        public string? OptionGroupName { get; set; }
        public string? OptionName { get; set; }
        public decimal? OUp { get; set; }
    }
}
