using prjMSIT145_Final.Models;

namespace prjMSIT145_Final.ViewModels
{
    public class VCUtilityViewModel
    {

        public List<VBusinessMemberContainImgViewModel>? BusinessMemberList { get; set; }
        public List<AdImg>? AdImgList { get; set; }
        //-----------
        public List<VBusinessMemberDetailViewModel>? BusinessMemberDetailList { get; set; }
        public List<VPaymenttermViewModel>? PaymenttermList { get; set; }
        public List<VProductClassViewModel>? ProductClassList { get; set; }
        public  int OrderID { get; set; }
        //-----------
        public List<VProductBasicInfoViewModel>? ProductBasicInfoList { get; set; }
        public List<VOptionGroupViewModel>? OptionGroupList { get; set; }
        //-----------
        public List<VOrdersViewModel>? OrdersList { get; set; }
    }
}
