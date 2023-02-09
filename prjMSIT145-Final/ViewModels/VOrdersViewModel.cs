namespace prjMSIT145_Final.ViewModels
{
    public class VOrdersViewModel
    {
        public int Fid { get; set; }
        public int? NFid { get; set; }
        public int? BFid { get; set; }
        public string? MemberName { get; set; }
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
        public string? OrderTimeMM { get; set; }
        public string? OrderTimedd { get; set; }
        public string? OrderTimeHH { get; set; }
        public string? OrderTimeminute { get; set; }
        public int? TotalAmount { get; set; }
        public string? OrderISerialId { get; set; }
    }
}
