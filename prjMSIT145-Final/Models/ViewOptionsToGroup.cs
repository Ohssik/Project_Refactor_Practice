using System;
using System.Collections.Generic;

namespace prjMSIT145_Final.Models
{
    public partial class ViewOptionsToGroup
    {
        public int Fid { get; set; }
        public string? OptionName { get; set; }
        public int? OptionGroupFid { get; set; }
        public string? OptionGroupName { get; set; }
        public decimal? UnitPrice { get; set; }
        public int? BFid { get; set; }
        public string? Memo { get; set; }
    }
}
