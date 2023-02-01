using prjMSIT145_Final.Models;
using System.ComponentModel;

namespace prjMSIT145_Final.ViewModels
{
    
    public class CABusinessMemberViewModel
    {
        private BusinessMember _businessMember;
        private IEnumerable<Order> _orders;
        public CABusinessMemberViewModel()
        {
            _businessMember = new BusinessMember();
            _orders = new List<Order>();
        }
        public BusinessMember businessMember
        {
            get { return _businessMember; }
            set { _businessMember = value; }
        }
        public IEnumerable<Order> orders
        {
            get { return _orders; }
            set { _orders = value; }
        }
        [DisplayName("ID")]
        public int Fid
        {
            get { return _businessMember.Fid; }
            set { _businessMember.Fid = value; }
        }
        [DisplayName("商家名稱")]
        public string? MemberName
        {
            get { return _businessMember.MemberName; }
            set { _businessMember.MemberName = value; }
        }
        [DisplayName("電話號碼")]
        public string? Phone
        {
            get { return _businessMember.Phone; }
            set { _businessMember.Phone = value; }
        }
        [DisplayName("開店時間")]
        public string? sOpenTime
        {
            get 
            {
                string openT = "";
                
                if(_businessMember.OpenTime!=null)
                    openT += ((TimeSpan)_businessMember.OpenTime).Hours+":"+((TimeSpan)_businessMember.OpenTime).Minutes+" - ";
                if (_businessMember.CloseTime!=null)
                    openT += ((TimeSpan)_businessMember.CloseTime).Hours+":"+((TimeSpan)_businessMember.CloseTime).Minutes;

                return openT;
            }
        }
        public TimeSpan? OpenTime
        {
            get { return _businessMember.OpenTime; }
            set { _businessMember.OpenTime = value; }
        }
        
        public TimeSpan? CloseTime
        {
            get { return _businessMember.CloseTime; }
            set { _businessMember.CloseTime = value; }
        }
        [DisplayName("地址")]
        public string? Address
        {
            get { return _businessMember.Address; }
            set { _businessMember.Address = value; }

        }
        [DisplayName("商家類別")]
        public string? ShopType
        {
            get { return _businessMember.ShopType; }
            set { _businessMember.ShopType = value; }
        }
        [DisplayName("聯絡人")]
        public string? ContactPerson
        {
            get { return _businessMember.ContactPerson; }
            set { _businessMember.ContactPerson = value; }
        }
        [DisplayName("帳號")]
        public string? MemberAccount
        {
            get { return _businessMember.MemberAccount; }
            set { _businessMember.MemberAccount = value; }
        }

        public string? Email
        {
            get { return _businessMember.Email; }
            set { _businessMember.Email = value; }
        }
        [DisplayName("註冊時間")]
        public DateTime? RegisterTime
        {
            get { return _businessMember.RegisterTime; }
            set { _businessMember.RegisterTime = value; }
        }
        [DisplayName("商家GPS")]
        public string? Gps
        {
            get { return _businessMember.Gps; }
            set { _businessMember.Gps = value; }
        }
        [DisplayName("帳號停權中")]
        public string? _IsSuspensed
        {
            get
            {
                string showValue = _businessMember.IsSuspensed>0 ? "V" : "";
                return showValue;
            }
        }
        public int? IsSuspensed
        {
            get { return _businessMember.IsSuspensed; }
            set { _businessMember.IsSuspensed = value; }
        }
        [DisplayName("Email認證")]        
        public int? EmailCertified
        {
            get { return _businessMember.EmailCertified; }
            set { _businessMember.EmailCertified = value; }
        }
        public int? IsOpened
        {
            get { return _businessMember.IsOpened; }
            set { _businessMember.IsOpened = value; }
        }
        public string? txtPassword { get; set; }
        public string? txtConfirmPwd { get; set; }
    }
}
