using prjMSIT145Final.Infrastructure.Models;

namespace prjMSIT145Final.Web.ViewModels
{
    public class VProductClassViewModel
    {
        public int Fid { get; set; }
        public string? CategoryName { get; set; }
        public List<VProductsViewModel>? ProductsList { get; set; }
        public int PlayregionID { get; set; }
    }
}
