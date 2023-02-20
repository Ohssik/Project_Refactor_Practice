using prjMSIT145_Final.Models;

namespace prjMSIT145_Final.ViewModels
{
    public class CASettingViewModel
    {
        private AdminMember _admin;
        public CASettingViewModel()
        {
            _admin = new AdminMember();
        }
        public AdminMember admin
        {
            get { return _admin; }
            set { _admin = value; }
        }
        public int Fid
        {
            get { return _admin.Fid; }
            set { _admin.Fid = value; }
        }
        public string? Account
        {
            get { return _admin.Account; }
            set { _admin.Account = value; }
        }
        public string? Email
        {
            get { return _admin.Email; }
            set { _admin.Email = value; }
        }
        public int? RoleLevel
        {
            get { return _admin.RoleLevel; }
            set { _admin.RoleLevel = value; }
        }
        public string? txtPassword { get; set; }
        public string? txtConfirmPwd { get; set; }
    }
}
