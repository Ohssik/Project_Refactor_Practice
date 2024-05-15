using prjMSIT145Final.Infrastructure.Models;
using prjMSIT145Final.Repository.Interfaces;
using prjMSIT145Final.Service.Dtos;
using prjMSIT145Final.Service.Interfaces;
using prjMSIT145Final.Service.ParameterDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjMSIT145Final.Service.Implements
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepo;

        public AdminService(IAdminRepository adminRepository
        )
        { 
            _adminRepo = adminRepository;
        }

        public Task<bool> CheckPwd(CheckPwdParameterDto parameter)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAd(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AdminMemberDto> Get(CheckPwdParameterDto parameter)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AdImg>> GetAllAd()
        {
            throw new NotImplementedException();
        }

        public Task ModifyAdInfo(AdImg ad)
        {
            throw new NotImplementedException();
        }

        public Task ModifyAdsOrderBy(IEnumerable<AdImg> ads)
        {
            throw new NotImplementedException();
        }

        public Task SendAccountLockedNotice(SendEmailParameterDto parameter)
        {
            throw new NotImplementedException();
        }

        public async Task<AdImg> AddUploadAdInfo(AdImg ad)
        {
            return await _adminRepo.AddUploadAdInfo(ad);
        }

        public Task<AdImg> UpdateUploadAdInfo(AdImg ad)
        {
            throw new NotImplementedException();
        }

        public Task<AdImg> GetAdByOrderBy(int orderBy)
        {
            throw new NotImplementedException();
        }

        public async Task<AdImg> GetAdById(int id)
        {
            return await _adminRepo.GetAdById(id);
        }

        public Task<bool> CheckAccountInfo(ForgetPwdParameterDto parameter)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePwd(ModifyPwdParameterDto parameter)
        {
            throw new NotImplementedException();
        }

        public async Task<string> AddChangePasswordRequest(ChangeRequestPassword resquest)
        {
            try
            {
                await _adminRepo.AddChangePasswordRequest(resquest);
                return "success";
            }
            catch(Exception ex)
            {
                return $"error:{ex.Message}";
            }
        }

        public async Task<ChangeRequestPassword> CheckTokenIsExpired(string token)
        {
            return await _adminRepo.CheckTokenIsExpired(token);
        }

        public Task<string> DeleteChangePwdRequest(ChangeRequestPassword resquest)
        {
            throw new NotImplementedException();
        }
    }
}
