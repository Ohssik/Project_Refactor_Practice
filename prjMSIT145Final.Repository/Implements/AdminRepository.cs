using Microsoft.EntityFrameworkCore;
using prjMSIT145Final.Infrastructure.Models;
using prjMSIT145Final.Repository.Interfaces;
using prjMSIT145Final.Repository.ParameterModels;
using System.Xml.Linq;
using static prjMSIT145Final.Infrastructure.Models.GoogleGeocodingAPI;

namespace prjMSIT145Final.Repository.Implements
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ispanMsit145shibaContext _context;

        public AdminRepository(
            ispanMsit145shibaContext context
            )
        {
            _context = context;
        }

        public async Task<bool> CheckPwd(CheckPwdParameterModel parameter)
        {
            var member = await _context.AdminMembers.FirstOrDefaultAsync(
                                u => u.Account == parameter.Account && u.Password == parameter.Password);

            return member != null;
        }

        public async Task DeleteAd(int id)
        {
            var deleteItem =  await _context.AdImgs.FirstOrDefaultAsync(a => a.Fid == id);

            if(deleteItem != null)
            {
                _context.AdImgs.Remove(deleteItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<AdminMember> Get(CheckPwdParameterModel parameter)
        {
            var member = await _context.AdminMembers.FirstOrDefaultAsync(
                u => u.Account == parameter.Account && u.Password == parameter.Password);

            return member ?? new AdminMember();
        }

        public async Task<IEnumerable<AdImg>> GetAllAd()
        {
            throw new NotImplementedException();
        }

        public async Task ModifyAdInfo(AdImg ad)
        {
            var updateItem = await  _context.AdImgs
                                    .FirstOrDefaultAsync(a => a.Fid == Convert.ToInt32(ad.Fid));
            if(updateItem != null)
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

        public async Task SendAccountLockedNotice(SendEmailParameterModel parameter)
        {
            if (parameter != null)
            {
                var DemoMailServer = _config["DemoMailServer:pwd"];

                if (parameter.MemberType == "B")
                {
                    var user = await _context.BusinessMembers
                                      .FirstOrDefaultAsync(t => t.Fid == parameter.MemberId);

                    if (user != null)
                    {
                        user.IsSuspensed = (int)parameter.IsSuspensed;
                    }
                }
                else if (parameter.MemberType == "N")
                {
                    var user = await _context.NormalMembers
                                   .FirstOrDefaultAsync(t => t.Fid == parameter.MemberId);

                    if (user != null)
                    {
                        user.IsSuspensed = (int)parameter.IsSuspensed;
                    }
                }

                await _context.SaveChangesAsync();

                string changeType = (int)parameter.IsSuspensed == 1 ? "停權" : "復權";
                string mailBody = $"您好：<br>您的帳戶已被{changeType}，若有問題請洽詢網站管理員。" +
                    "<br><br>" +
                    $"{changeType}原因：<br>" +
                    parameter.TxtMessage +
                    "<br><br><hr><br>" +
                    "<br>此為系統通知，請勿直接回信，謝謝";

                string mailSubject = $"帳號{changeType}通知";

                //string result = sendMail(mail.txtRecipient, mailBody, mailSubject);
                string result = (new CSendMail()).sendMail(parameter.TxtRecipient, mailBody, mailSubject, _config["DemoMailServer:pwd"].ToString());
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

        public async Task UpdatePwd(ModifyPwdParameterModel parameter)
        {
            var updateItem = await _context.AdminMembers.FirstOrDefaultAsync(a => a.Fid == parameter.Fid);

            if(updateItem != null)
            {
                updateItem.Password = parameter.Password;
                updateItem.Email = parameter.Email;

                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> CheckAccountInfo(ForgetPwdParameterModel parameter)
        {
            var user = await _context.AdminMembers.FirstOrDefaultAsync
                            (u => u.Account == parameter.txtAccount && u.Email == parameter.txtEmail);

            return user != null;
        }

        public async Task AddChangePasswordRequest(ChangeRequestPassword resquest)
        {

            try
            {
                await _context.ChangeRequestPasswords.AddAsync(resquest);
                await _context.SaveChangesAsync();
            }
            catch (Exception err)
            {
                throw;
            }
        }

        public async Task<ChangeRequestPassword> CheckTokenIsExpired(string token)
        {
            var result = await _context.ChangeRequestPasswords.FirstOrDefaultAsync(r => r.Token == token);
            return result ?? new ChangeRequestPassword();
        }

        public async Task<string> DeleteChangePwdRequest(ChangeRequestPassword resquest)
        {
            var result = "";
            var request = await _context.ChangeRequestPasswords.FirstOrDefaultAsync(r => r.Token == resquest.Token);

            if (request != null)
            {
                var deleteItems = _context.ChangeRequestPasswords.Where(r => r.Account == resquest.Account);
                await Task.Run(()=>_context.ChangeRequestPasswords.RemoveRange(deleteItems.ToList()));

                try
                {
                    await _context.SaveChangesAsync();
                    result = "success";
                }
                catch (Exception err)
                {
                    result = $"error:{err.Message}";
                }

            }

            return result;

        }
    }
}
