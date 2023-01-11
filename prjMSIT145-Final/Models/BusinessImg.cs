using System;
using System.Collections.Generic;

namespace prjMSIT145_Final.Models
{
    public partial class BusinessImg
    {
        public int Fid { get; set; }
        public int? BFid { get; set; }
        public string? LogoImgFileName { get; set; }
        public string? SighImgFileName { get; set; }
        public string? BannerImgFileName1 { get; set; }
        public string? BannerImgFileName2 { get; set; }
        public string? BannerImgFileName3 { get; set; }
    }
}
