using System;
using System.Collections.Generic;

namespace prjMSIT145_Final.Models
{
    public partial class Product
    {
        public int Fid { get; set; }
        public int BFid { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal? UnitPrice { get; set; }
        public int? Qty { get; set; }
        public int? CategoryFid { get; set; }
        public string? Memo { get; set; }
        public string? MenuFid { get; set; }
        public string? IsForSale { get; set; }
    }
}
