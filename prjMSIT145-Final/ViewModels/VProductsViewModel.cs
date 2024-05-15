namespace prjMSIT145Final.Web.ViewModels
{
    public class VProductsViewModel
    {
        public int Fid { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal? UnitPrice { get; set; }
        public int? IsForSale { get; set; }
        public string? Photo { get; set; }
        public int? OrdetAmount { get;set; }
    }
}
