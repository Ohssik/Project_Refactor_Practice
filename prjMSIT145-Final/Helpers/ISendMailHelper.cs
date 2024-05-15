using prjMSIT145Final.Web.ViewModels;
using static prjMSIT145Final.Helpers.SendMailHelper;

namespace prjMSIT145Final.Helpers
{
    public interface ISendMailHelper
    {
        Task<MailContentModel> SetForgetPwdMailContent(ForgetPwdParameter parameter, string url, string token);

        Task<string> SendMail(MailContentModel mail);
    }
}
