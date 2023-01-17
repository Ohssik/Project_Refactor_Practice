using prjMSIT145_Final.Models;
using System.ComponentModel;

namespace prjMSIT145_Final.ViewModels
{
    public class CANormalMemberViewModel
    {
        private NormalMember _normalMember;
        private IEnumerable<Order> _orders;
        public CANormalMemberViewModel()
        {
            _normalMember = new NormalMember();
            _orders = new List<Order>();
        }
        public NormalMember normalMember
        {
            get { return _normalMember; }
            set { _normalMember = value; }
        }
        public IEnumerable<Order> orders
        {
            get { return _orders; }
            set { _orders = value; }
        }
        [DisplayName("ID")]
        public int Fid
        {
            get { return _normalMember.Fid; }
            set { _normalMember.Fid = value; }
        }
        [DisplayName("會員姓名")]
        public string? MemberName
        {
            get { return _normalMember.MemberName; }
            set { _normalMember.MemberName = value; }
        }
        [DisplayName("手機號碼")]
        public string? Phone
        {
            get { return _normalMember.Phone; }
            set { _normalMember.Phone = value; }
        }
        [DisplayName("性別")]
        public string? Gender
        {
            get { return _normalMember.Gender; }
            set { _normalMember.Gender = value; }
        }
        [DisplayName("居住地")]
        public string? Address
        {
            get { return _normalMember.AddressCity+_normalMember.AddressArea; }            
        }
        //public string? AddressCity
        //{
        //    get { return _normalMember.AddressCity; }
        //    set { _normalMember.AddressCity = value; }
        //}
        //public string? AddressArea
        //{
        //    get { return _normalMember.AddressArea; }
        //    set { _normalMember.AddressArea = value; }
        //}
        [DisplayName("生日")]
        public DateTime? Birthday
        {
            get { return _normalMember.Birthday; }
            set { _normalMember.Birthday = value; }
        }
        
        public string? Email
        {
            get { return _normalMember.Email; }
            set { _normalMember.Email = value; }
        }
        [DisplayName("註冊時間")]
        public DateTime? RegisterTime
        {
            get { return _normalMember.RegisterTime; }
            set { _normalMember.RegisterTime = value; }
        }
        public string? MemberPhotoFile
        {
            get { return _normalMember.MemberPhotoFile; }
            set { _normalMember.MemberPhotoFile = value; }
        }
        [DisplayName("是否停權")]
        public string? _IsSuspensed
        {
            get
            {
                string showValue = _normalMember.IsSuspensed>0 ? "V" : "";
                return showValue;
            }
        }
        public int? IsSuspensed
        {
            get { return _normalMember.IsSuspensed; }
            set { _normalMember.IsSuspensed = value; }
        }
        [DisplayName("Email認證")]
        public string? _EmailCertified
        {
            get
            {
                string showValue = _normalMember.EmailCertified>0 ? "O" : "X";
                return showValue;
            }
        }
        public int? EmailCertified
        {
            get { return _normalMember.EmailCertified; }
            set { _normalMember.EmailCertified = value; }
        }
        public string? txtPassword { get; set; }
        public string? txtConfirmPwd { get; set; }
    }
}
