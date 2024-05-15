namespace prjMSIT145Final.Web.ViewModel
{
    public class COrderItemViewModel
    {
        //public int? ProductFid { get; set; }
        public string? ProductName { get; set; }
        public decimal? Productprice { get; set; }
        public int? Qty { get; set; }
        public int? Fid { get; set; }
        //detail
        //public int? OptionFid { get; set; }
        public List<string>? OptionName { get; set; }
        public decimal? OptionPrice { get; set; }
        //總價
        public decimal? OptionAmount { get { return (OptionPrice + Productprice); } }
    }
}
