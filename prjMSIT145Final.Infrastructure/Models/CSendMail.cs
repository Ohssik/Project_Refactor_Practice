using System.Net.Mail;

namespace prjMSIT145Final.Infrastructure.Models
{
    public class CSendMail
    {
        public string sendMail(string email, string mailBody, string mailSubject,string mailServer)//發信
        {
            //var DemoMailServer = _config["DemoMailServer:pwd"];
            string DemoMailServer = mailServer;
            MailMessage MyMail = new MailMessage();
            MyMail.From = new MailAddress("ShibaAdmin@msit145shiba.com.tw", "日柴", System.Text.Encoding.UTF8);
            MyMail.To.Add(email);

            MyMail.Subject = mailSubject;
            MyMail.Body = mailBody; //設定信件內容
            MyMail.IsBodyHtml = true; //是否使用html格式

            SmtpClient MySMTP = new SmtpClient();
            //MySMTP.UseDefaultCredentials = true;
            MySMTP.Credentials = new System.Net.NetworkCredential("b9809004@gapps.ntust.edu.tw", DemoMailServer); //這裡要填正確的帳號跟密碼
            MySMTP.Host = "smtp.gmail.com"; //設定smtp Server
            MySMTP.Port = 587;
            MySMTP.EnableSsl = true; //gmail預設開啟驗證
            try
            {
                MySMTP.Send(MyMail);
                return "success";
            }
            catch (Exception ex)
            {
                return $"Mail error:{ex.ToString()}";
            }
            finally
            {
                MyMail.Dispose(); //釋放資源
            }

        }
    }
}
