using prjMSIT145_Final.ViewModels;

namespace prjMSIT145_Final.Models
{
    public class CUtility
    {
        public static List<VBusinessMemberContainImgViewModel>? BusinessMemberList = null;
        //-----------
        public static List<VBusinessMemberDetailViewModel>? BusinessMemberDetailList = null;
        public static List<VPaymenttermViewModel>? PaymenttermList = null;
        public static List<VProductClassViewModel>? ProductClassList = null;
        //-----------
        public static List<VProductBasicInfoViewModel>? ProductBasicInfoList = null;
        public static List<VOptionGroupViewModel>? OptionGroupList = null;
        //-----------
        public static List<VOrdersViewModel>? OrdersList = null;

        public static int CategoryID = 0;
        public static int ProductID = 0;
        public static int OrderitemID = 1; //初值必須為1
        public static int ProductCount = 0;
        public static int NormalMemberID = 0; //測試時使用1
    }
}
