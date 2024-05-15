namespace prjMSIT145Final.Web.ViewModels
{
    public class VOptionGroupViewModel
    {
        public int Fid { get; set; }
        public string? OptionGroupName { get; set; }
        public int? IsMultiple { get; set; }
        public List<VOptionViewModel> OptionList { get; set; }
    }
}
