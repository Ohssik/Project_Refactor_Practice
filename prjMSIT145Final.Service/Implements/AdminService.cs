using MapsterMapper;
using prjMSIT145Final.Infrastructure.Models;
using prjMSIT145Final.Repository.Interfaces;
using prjMSIT145Final.Repository.ParameterModels;
using prjMSIT145Final.Service.Dtos;
using prjMSIT145Final.Service.Interfaces;
using prjMSIT145Final.Service.ParameterDtos;

namespace prjMSIT145Final.Service.Implements
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepo;
        private readonly IMapper _mapper;

        public AdminService(IAdminRepository adminRepository
            ,IMapper mapper)
        { 
            _adminRepo = adminRepository;
            _mapper = mapper;
        }

        public async Task<AdminMemberDto> Get(CheckPwdParameterDto parameter)
        {
            var model = await _adminRepo.Get(_mapper.Map<CheckPwdParameterModel>(parameter));
            var result = _mapper.Map<AdminMemberDto>(model);
            return result;
        }

        public async Task SendAccountLockedNotice(SendEmailParameterDto parameter)
        {
            var parameterModel = _mapper.Map<SendEmailParameterModel>(parameter);
            await _adminRepo.SendAccountLockedNotice(parameterModel);
        }

        public async Task<bool> CheckAccountInfo(ForgetPwdParameterDto parameter)
        {
            var parameterModel = _mapper.Map<ForgetPwdParameterModel>(parameter);
            return await _adminRepo.CheckAccountInfo(parameterModel);
        }

        public async Task UpdatePwd(ModifyPwdParameterDto parameter)
        {
            var parameterModel = _mapper.Map<ModifyPwdParameterModel>(parameter);
            await _adminRepo.UpdatePwd(parameterModel);
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

        public async Task<string> DeleteChangePwdRequest(ChangeRequestPassword resquest)
        {
            return await _adminRepo.DeleteChangePwdRequest(resquest);
        }
    }
}
