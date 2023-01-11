using System;
using System.Collections.Generic;

namespace prjMSIT145_Final.Models
{
    public partial class Coupon
    {
        public int Id { get; set; }
        public string? CouponCode { get; set; }
        public decimal? Price { get; set; }
        public int? IsUsed { get; set; }
    }
}
