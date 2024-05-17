using Microsoft.EntityFrameworkCore;
using prjMSIT145Final.Infrastructure.Models;
using prjMSIT145Final.Repository.Interfaces;

namespace prjMSIT145Final.Repository.Implements
{
    public class AdImgRepository: IAdImgRepository
    {
        private readonly ispanMsit145shibaContext _context;

        public AdImgRepository(
            ispanMsit145shibaContext context
        )
        { 
            _context = context;
        }

        public async Task DeleteAd(int id)
        {
            var deleteItem = await _context.AdImgs.FirstOrDefaultAsync(a => a.Fid == id);

            if (deleteItem != null)
            {
                _context.AdImgs.Remove(deleteItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<AdImg>> GetAllAd()
        {
            var result = await Task.Run(() => _context.AdImgs.Select(img => img));
            return result ?? Enumerable.Empty<AdImg>();
        }

        public async Task ModifyAdInfo(AdImg ad)
        {
            var updateItem = await _context.AdImgs
                                    .FirstOrDefaultAsync(a => a.Fid == Convert.ToInt32(ad.Fid));
            if (updateItem != null)
            {
                updateItem.StartTime = ad.StartTime;
                updateItem.EndTime = ad.EndTime;
                updateItem.Hyperlink = ad.Hyperlink;

                await _context.SaveChangesAsync();
            }
        }

        public async Task ModifyAdOrderBy(int id, int orderBy)
        {
            var updateItem = await _context.AdImgs.FirstOrDefaultAsync(a => a.Fid == id);

            if (updateItem != null)
            {
                updateItem.OrderBy = orderBy;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<AdImg> AddUploadAdInfo(AdImg ad)
        {
            var lastAd = await _context.AdImgs.OrderBy(a => a.OrderBy).LastOrDefaultAsync();
            int orderBy = 1;

            if (lastAd != null)
            {
                orderBy = (int)lastAd.OrderBy + 1;
            }

            ad.OrderBy = orderBy;
            await _context.AdImgs.AddAsync(ad);
            await _context.SaveChangesAsync();

            return await GetAdByOrderBy(ad.OrderBy.GetValueOrDefault());
        }

        public async Task<AdImg> GetAdByOrderBy(int orderBy)
        {
            return await _context.AdImgs.FirstOrDefaultAsync(ad => ad.OrderBy == orderBy)
                ?? new AdImg();
        }

        public async Task<AdImg> UpdateUploadAdInfo(AdImg ad)
        {
            var lastAd = await _context.AdImgs.FirstOrDefaultAsync(a => a.Fid == ad.Fid);

            if (lastAd != null)
            {
                lastAd.Hyperlink = ad.Hyperlink;
                lastAd.ImgName = ad.ImgName;

                await _context.SaveChangesAsync();
            }

            return lastAd ?? new AdImg();
        }

        public async Task<AdImg> GetAdById(int id)
        {
            return await _context.AdImgs.FirstOrDefaultAsync(i => i.Fid == id)
                ?? new AdImg();
        }

    }
}
