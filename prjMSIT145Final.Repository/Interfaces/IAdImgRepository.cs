using prjMSIT145Final.Infrastructure.Models;

namespace prjMSIT145Final.Repository.Interfaces
{
    public interface IAdImgRepository
    {
        Task<IEnumerable<AdImg>> GetAllAd();

        Task ModifyAdOrderBy(int id, int orderBy);

        Task ModifyAdInfo(AdImg ad);

        Task DeleteAd(int id);

        Task<AdImg> AddUploadAdInfo(AdImg ad);

        Task<AdImg> UpdateUploadAdInfo(AdImg ad);

        Task<AdImg> GetAdByOrderBy(int orderBy);

        Task<AdImg> GetAdById(int id);

    }
}
