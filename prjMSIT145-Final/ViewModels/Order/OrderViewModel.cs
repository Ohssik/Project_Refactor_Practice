namespace prjMSIT145Final.ViewModels.Order
{
    public class OrderViewModel
    {
        public int Fid { get; set; }
        public int? NFid { get; set; }
        public int? BFid { get; set; }
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

    }
}
