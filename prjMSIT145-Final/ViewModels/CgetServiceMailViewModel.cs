using System.ComponentModel;
namespace prjMSIT145_Final.ViewModels
{
    public class CgetServiceMailViewModel
    {
        public CgetServiceMailViewModel()
        {

        }
        [DisplayName("ID")]
        public int Fid { get; set; }
        public string? Email { get; set; }
        [DisplayName("發信人")]
        public string? SenderName { get; set; }
        [DisplayName("電話")]
        public string? Phone { get; set; }
        [DisplayName("詢問主題")]
        public string? Subject { get; set; }
        [DisplayName("詢問內容")]
        public string? Context { get; set; }
        [DisplayName("收信時間")]
        public DateTime? ReceivedTime { get; set; }
        [DisplayName("已讀時間")]
        public DateTime? ReadTime { get; set; }
        [DisplayName("回覆內容")]
        public string? Reply { get; set; }
    }
}
