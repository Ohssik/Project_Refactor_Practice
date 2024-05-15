using prjMSIT145Final.Web.ViewModels;

namespace prjMSIT145Final.Infrastructure.Models
{
    public class CUtility
    {
        public static List<VBusinessMemberContainImgViewModel>? BusinessMemberList = null;
        public static List<AdImg>? AdImgList = null;
        //-----------
        public static List<VBusinessMemberDetailViewModel>? BusinessMemberDetailList = null;
        public static List<VPaymenttermViewModel>? PaymenttermList = null;
        public static List<VProductClassViewModel>? ProductClassList = null;
        public static int OrderID = 0;
        public static int OrderitemID = 0; 
        //-----------
        public static List<VProductBasicInfoViewModel>? ProductBasicInfoList = null;
        public static List<VOptionGroupViewModel>? OptionGroupList = null;
        //-----------
        public static List<VOrdersViewModel>? OrdersList = null;
    }
}
