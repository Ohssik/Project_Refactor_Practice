using prjMSIT145Final.Models;
using prjMSIT145Final.Web.ViewModels;

namespace prjMSIT145Final.Helpers
{
    public interface ISendMailHelper
    {
        Task<MailContentModel> SetForgetPwdMailContent(ForgetPwdParameter parameter, string url, string token);

        Task<MailContentModel> SetAccountLockedNoticeContent(SendEmailParameter parameter);

        Task<string> SendMail(MailContentModel mail);
    }
}
