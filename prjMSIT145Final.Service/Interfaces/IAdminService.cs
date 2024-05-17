using prjMSIT145Final.Infrastructure.Models;
using prjMSIT145Final.Service.Dtos;
using prjMSIT145Final.Service.ParameterDtos;

namespace prjMSIT145Final.Service.Interfaces
{
    public interface IAdminService
    {
        Task SendAccountLockedNotice(SendEmailParameterDto parameter);

        Task<bool> CheckAccountInfo(ForgetPwdParameterDto parameter);

        Task UpdatePwd(ModifyPwdParameterDto parameter);

        Task<AdminMemberDto> Get(CheckPwdParameterDto parameter);

        Task<string> AddChangePasswordRequest(ChangeRequestPassword resquest);
        Task<string> DeleteChangePwdRequest(ChangeRequestPassword resquest);

        Task<ChangeRequestPassword> CheckTokenIsExpired(string token);

    }
}
