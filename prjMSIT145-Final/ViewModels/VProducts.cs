namespace prjMSIT145_Final.ViewModels
{
    public class VProducts
    {
        public int Fid { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal? UnitPrice { get; set; }
        public int? IsForSale { get; set; }
        public string? Photo { get; set; }
        public int OrdetAmount { get; set; }
    }
}
