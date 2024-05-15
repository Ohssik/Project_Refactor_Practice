using System.Net.Mail;

namespace prjMSIT145Final.Infrastructure.Models
{
    public class Gmail
    {
        public void sendGmail(string sendto,string emailUrl)
        {
            MailMessage mail = new MailMessage();
            //前面是發信email後面是顯示的名稱
            mail.From = new MailAddress("ms60416@gmail.com", "日柴Daily驗證信");

            //收信者email
            mail.To.Add(sendto);

            //設定優先權
            mail.Priority = MailPriority.Normal;

            //標題
            mail.Subject = "企業驗證信";

            //內容
            //mail.Body = $"親愛的使用者你好 以下是你的註冊網址 https://localhost:7266/BusinessMember/Register/?email={sendto}   日柴Daily";
            mail.Body = $"親愛的使用者你好 以下是你的註冊網址 {emailUrl}/BusinessMember/Register/?email={sendto}   日柴Daily";

            //內容使用html
            mail.IsBodyHtml = true;

            //設定gmail的smtp (這是google的)
            SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587);

            //您在gmail的帳號密碼
            MySmtp.Credentials = new System.Net.NetworkCredential("b9809004@gapps.ntust.edu.tw", "Gknt824nut");

            //開啟ssl
            MySmtp.EnableSsl = true;

            //發送郵件
            MySmtp.Send(mail);

            //放掉宣告出來的MySmtp
            MySmtp = null;

            //放掉宣告出來的mail
            mail.Dispose();
        }






    }
}

