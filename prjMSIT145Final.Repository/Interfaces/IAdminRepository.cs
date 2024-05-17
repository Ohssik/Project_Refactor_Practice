using prjMSIT145Final.Infrastructure.Models;
using prjMSIT145Final.Repository.ParameterModels;

namespace prjMSIT145Final.Repository.Interfaces
{
    public interface IAdminRepository
    {
        Task SendAccountLockedNotice(SendEmailParameterModel parameter);

        Task<bool> CheckAccountInfo(ForgetPwdParameterModel parameter);

        Task UpdatePwd(ModifyPwdParameterModel parameter);

        Task<AdminMember> Get(CheckPwdParameterModel parameter);

        Task AddChangePasswordRequest(ChangeRequestPassword resquest);

        Task<string> DeleteChangePwdRequest(ChangeRequestPassword resquest);

        Task<ChangeRequestPassword> CheckTokenIsExpired(string token);
    }
}
