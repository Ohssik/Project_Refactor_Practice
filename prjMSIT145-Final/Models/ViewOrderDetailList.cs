using System;
using System.Collections.Generic;

namespace prjMSIT145_Final.Models
{
    public partial class ViewOrderDetailList
    {
        public string? Itemid { get; set; }
        public int? ProductFId { get; set; }
        public string? ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
        public int? Qty { get; set; }
        public string? Options { get; set; }
        public decimal? SubTotal { get; set; }
    }
}
