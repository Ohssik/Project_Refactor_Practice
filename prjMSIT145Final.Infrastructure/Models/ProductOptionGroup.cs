using System;
using System.Collections.Generic;

namespace prjMSIT145Final.Infrastructure.Models
{
    public partial class ProductOptionGroup
    {
        public int Fid { get; set; }
        public string? OptionGroupName { get; set; }
        public int? BFid { get; set; }
        public string? Memo { get; set; }
        public int? IsMultiple { get; set; }
    }
}
