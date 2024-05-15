using System;
using System.Collections.Generic;

namespace prjMSIT145Final.Infrastructure.Models
{
    public partial class Coupon
    {
        public int Fid { get; set; }
        public string? CouponCode { get; set; }
        public decimal? Price { get; set; }
        public int? IsUsed { get; set; }
        public string? Memo { get; set; }
        public string? Title { get; set; }
    }
}
