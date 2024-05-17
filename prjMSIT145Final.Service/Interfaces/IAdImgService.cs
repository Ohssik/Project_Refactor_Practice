using prjMSIT145Final.Infrastructure.Models;

namespace prjMSIT145Final.Service.Interfaces
{
    public interface IAdImgService
    {
        Task<IEnumerable<AdImg>> GetAllAd();

        Task ModifyAdsOrderBy(IEnumerable<AdImg> ads);

        Task ModifyAdInfo(AdImg ad);

        Task DeleteAd(int id);

        Task<AdImg> AddUploadAdInfo(AdImg ad);

        Task<AdImg> UpdateUploadAdInfo(AdImg ad);

        Task<AdImg> GetAdById(int id);


    }
}
