using System;
using System.Collections.Generic;

namespace prjMSIT145_Final.Models
{
    public partial class ViewShowProductList
    {
        public int Fid { get; set; }
        public int BFid { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal? UnitPrice { get; set; }
        public int? CategoryFid { get; set; }
        public string? CategoryName { get; set; }
        public string? Memo { get; set; }
        public string? IsForSale { get; set; }
    }
}
