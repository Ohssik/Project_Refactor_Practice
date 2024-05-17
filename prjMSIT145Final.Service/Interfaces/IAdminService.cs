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

        Task<IEnumerable<AdImg>> GetAllAd();

        Task ModifyAdsOrderBy(IEnumerable<AdImg> ads);

        Task ModifyAdInfo(AdImg ad);

        Task DeleteAd(int id);

        Task<AdImg> AddUploadAdInfo(AdImg ad);

        Task<AdImg> UpdateUploadAdInfo(AdImg ad);

        Task<AdImg> GetAdById(int id);

        Task<string> AddChangePasswordRequest(ChangeRequestPassword resquest);
        Task<string> DeleteChangePwdRequest(ChangeRequestPassword resquest);

        Task<ChangeRequestPassword> CheckTokenIsExpired(string token);

    }
}
