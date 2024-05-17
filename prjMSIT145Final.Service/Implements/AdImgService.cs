using prjMSIT145Final.Infrastructure.Models;
using prjMSIT145Final.Repository.Interfaces;
using prjMSIT145Final.Service.Interfaces;

namespace prjMSIT145Final.Service.Implements
{
    public class AdImgService: IAdImgService
    {
        private readonly IAdImgRepository _adImgRepo;
        public AdImgService(
            IAdImgRepository adImgRepository
        )
        {
            _adImgRepo = adImgRepository;
        }

        public async Task<IEnumerable<AdImg>> GetAllAd()
        {
            return await _adImgRepo.GetAllAd();
        }

        public async Task ModifyAdInfo(AdImg ad)
        {
            await _adImgRepo.ModifyAdInfo(ad);
        }

        public async Task ModifyAdsOrderBy(IEnumerable<AdImg> ads)
        {
            foreach (var ad in ads)
            {
                if (ad.OrderBy.HasValue)
                {
                    await _adImgRepo.ModifyAdOrderBy(ad.Fid, ad.OrderBy.GetValueOrDefault());
                }
            }
        }

        public async Task<AdImg> AddUploadAdInfo(AdImg ad)
        {
            return await _adImgRepo.AddUploadAdInfo(ad);
        }

        public async Task<AdImg> UpdateUploadAdInfo(AdImg ad)
        {
            return await _adImgRepo.UpdateUploadAdInfo(ad);
        }

        public async Task<AdImg> GetAdById(int id)
        {
            return await _adImgRepo.GetAdById(id);
        }

        public async Task DeleteAd(int id)
        {
            await _adImgRepo.DeleteAd(id);
        }
    }
}
