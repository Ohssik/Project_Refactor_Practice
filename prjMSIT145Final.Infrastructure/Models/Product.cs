using System;
using System.Collections.Generic;

namespace prjMSIT145Final.Infrastructure.Models
{
    public partial class Product
    {
        public int Fid { get; set; }
        public int BFid { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal? UnitPrice { get; set; }
        public int? CategoryFid { get; set; }
        public string? Memo { get; set; }
        public int? IsForSale { get; set; }
        public string? Photo { get; set; }
    }
}
