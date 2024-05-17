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
        Task<IEnumerable<AdImg>> GetAllAd();

        Task ModifyAdOrderBy(int id,int orderBy);

        Task ModifyAdInfo(AdImg ad);

        Task DeleteAd(int id);

        Task<AdImg> AddUploadAdInfo(AdImg ad);

        Task<AdImg> UpdateUploadAdInfo(AdImg ad);

        Task<AdImg> GetAdByOrderBy(int orderBy);

        Task<AdImg> GetAdById(int id);

        Task AddChangePasswordRequest(ChangeRequestPassword resquest);
        Task<string> DeleteChangePwdRequest(ChangeRequestPassword resquest);


        Task<ChangeRequestPassword> CheckTokenIsExpired(string token);
    }
}
