using System;
using System.Collections.Generic;

namespace prjMSIT145_Final.Models
{
    public partial class ProductImg
    {
        public int Fid { get; set; }
        public int? ProductFid { get; set; }
        public string? ImgPath { get; set; }
    }
}
