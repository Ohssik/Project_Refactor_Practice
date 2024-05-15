

using System.ComponentModel;

namespace prjMSIT145Final.Web.ViewModels
{
    public class CustomerServiceMailBoxViewModel
    {
        [DisplayName("姓名")]
        public string? txtSenderName { get; set; }
        [DisplayName("Email")]
        public string? txtEmailAddress { get; set; }
        [DisplayName("電話號碼")]
        public string? txtPhone { get; set; }
        [DisplayName("詢問主題")]
        public string? txtMailSubject { get; set; }
        [DisplayName("詢問內容")]
        public string? txtMailContent { get; set; }

    }
}
