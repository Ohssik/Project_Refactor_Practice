using System;
using System.Collections.Generic;

namespace prjMSIT145_Final.Models
{
    public partial class ProductCategory
    {
        public int Fid { get; set; }
        public string? CategoryName { get; set; }
        public int? BFid { get; set; }
    }
}
