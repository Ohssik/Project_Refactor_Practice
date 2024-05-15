namespace prjMSIT145Final.Web.ViewModels
{
    public class VProductBasicInfoViewModel
    {
        public int Fid { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal? UnitPrice { get; set; }
        public string? Memo { get; set; }
        public string? Photo { get; set; }
    }
}
