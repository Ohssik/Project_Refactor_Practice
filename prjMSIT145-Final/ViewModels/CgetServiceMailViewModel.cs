using prjMSIT145Final.Infrastructure.Models;
using System.ComponentModel;
namespace prjMSIT145Final.Web.ViewModels
{
    public class CgetServiceMailViewModel
    {
        private ServiceMailBox _serviceMail;
        
        public CgetServiceMailViewModel()
        {
            _serviceMail=new ServiceMailBox();
        }
        public ServiceMailBox serviceMail
        {
            get { return _serviceMail; }
            set { _serviceMail = value; }
        }
        [DisplayName("ID")]
        public int Fid
        {
            get { return _serviceMail.Fid; }
            set { _serviceMail.Fid = value; }
        }
        public string? Email
        {
            get { return _serviceMail.Email; }
            set { _serviceMail.Email = value; }
        }
        [DisplayName("發信人")]
        public string? SenderName
        {
            get { return _serviceMail.SenderName; }
            set { _serviceMail.SenderName = value; }
        }
        [DisplayName("電話")]
        public string? Phone
        {
            get { return _serviceMail.Phone; }
            set { _serviceMail.Phone = value; }
        }
        [DisplayName("詢問主題")]
        public string? Subject
        {
            get { return _serviceMail.Subject; }
            set { _serviceMail.Subject = value; }
        }
        [DisplayName("詢問內容")]
        public string? Context
        {
            get { return _serviceMail.Context; }
            set { _serviceMail.Context = value; }
        }
        [DisplayName("收信時間")]
        public DateTime? ReceivedTime
        {
            get { return _serviceMail.ReceivedTime; }
            set { _serviceMail.ReceivedTime = value; }
        }
        [DisplayName("已讀時間")]
        public DateTime? ReadTime
        {
            get { return _serviceMail.ReadTime; }
            set { _serviceMail.ReadTime = value; }
        }
        [DisplayName("回覆內容")]
        public string? Reply
        {
            get { return _serviceMail.Reply; }
            set { _serviceMail.Reply = value; }
        }
    }
}
