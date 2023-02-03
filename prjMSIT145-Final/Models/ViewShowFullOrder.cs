using System;
using System.Collections.Generic;

namespace prjMSIT145_Final.Models
{
    public partial class ViewShowFullOrder
    {
        public int OrderFid { get; set; }
        public int? Fid { get; set; }
        public int? NFid { get; set; }
        public int? BFid { get; set; }
        public string? BMemberName { get; set; }
        public string? BMemberPhone { get; set; }
        public DateTime? PickUpDate { get; set; }
        public TimeSpan? PickUpTime { get; set; }
        public string? PickUpType { get; set; }
        public string? PickUpPerson { get; set; }
        public string? PickUpPersonPhone { get; set; }
        public int? PayTermCatId { get; set; }
        public string? TaxIdnum { get; set; }
        public string? OrderState { get; set; }
        public string? Memo { get; set; }
        public DateTime? OrderTime { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? OrderISerialId { get; set; }
        public int? ItemFid { get; set; }
        public int? ProductFId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal? UnitPrice { get; set; }
        public int? Qty { get; set; }
        public string? Options { get; set; }
        public decimal? SubTotal { get; set; }
    }
}
