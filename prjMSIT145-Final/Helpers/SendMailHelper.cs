using prjMSIT145Final.Models;
using prjMSIT145Final.Web.ViewModels;
using System.Net.Mail;

namespace prjMSIT145Final.Helpers
{
    public class SendMailHelper : ISendMailHelper
    {
        public async Task<MailContentModel> SetForgetPwdMailContent(ForgetPwdParameter parameter, string url, string token)
        {
            string mailBody = $@"您好：<br>我們收到了您發送的忘記密碼通知。<br> 
                                請確認是您本人發出的請求後，請在<label style='color:red'><b>10分鐘內</b></label>點擊以下網址連結到修改密碼的頁面後輸入新密碼。
                                <br>如果您沒有發出請求，則可忽略此信。<br><br>
                                <a href='{url}' target='_blank'>★★★修改密碼★★★</a><br><br>
                                <hr>
                                <br><br>此為系統通知，請勿直接回信，謝謝";

            string mailSubject = $"修改密碼通知";

            return new MailContentModel
            {
                MailAddress = parameter?.txtEmail,
                MailBody = mailBody,
                MailSubject = mailSubject,
                Token = token
            };
        }

        /// <summary>
        /// 發信
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        public async Task<string> SendMail(MailContentModel mail)
        {
            var DemoMailServer = mail.MailServer;
            var MyMail = new MailMessage();
            MyMail.From = new MailAddress("ShibaAdmin@msit145shiba.com.tw", "日柴", System.Text.Encoding.UTF8);
            MyMail.To.Add(mail.MailAddress);

            MyMail.Subject = mail.MailSubject;
            MyMail.Body = mail.MailBody; //設定信件內容
            MyMail.IsBodyHtml = true; //是否使用html格式

            var MySMTP = new SmtpClient();
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
                return $"Mail error:{ex}";
            }
            finally
            {
                MyMail.Dispose(); //釋放資源
            }

        }

        public async Task<MailContentModel> SetAccountLockedNoticeContent(SendEmailParameter parameter)
        {
            string changeType = (int)parameter.IsSuspensed == 1 ? "停權" : "復權";
            string mailBody = $"您好：<br>您的帳戶已被{changeType}，若有問題請洽詢網站管理員。" +
                "<br><br>" +
                $"{changeType}原因：<br>" +
                parameter.TxtMessage +
                "<br><br><hr><br>" +
                "<br>此為系統通知，請勿直接回信，謝謝";

            string mailSubject = $"帳號{changeType}通知";

            return new MailContentModel
            {
                MailAddress = parameter?.TxtRecipient,
                MailBody = mailBody,
                MailSubject = mailSubject,
                MailServer = /*_config["DemoMailServer:pwd"].ToString()*/""
            };

        }
    }
}
