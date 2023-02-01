using prjMSIT145_Final.Models;

namespace prjMSIT145_Final.ViewModels
{
    public class VProductClassViewModel
    {
        public int Fid { get; set; }
        public string? CategoryName { get; set; }
        public List<VProductsViewModel>? ProductsList { get; set; }
    }
}
