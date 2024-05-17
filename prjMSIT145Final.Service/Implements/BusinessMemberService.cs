using MapsterMapper;
using prjMSIT145Final.Infrastructure.Models;
using prjMSIT145Final.Repository.Interfaces;
using prjMSIT145Final.Repository.ParameterModels;
using prjMSIT145Final.Service.Dtos;
using prjMSIT145Final.Service.Interfaces;
using prjMSIT145Final.Service.ParameterDtos;

namespace prjMSIT145Final.Service.Implements
{
    public class BusinessMemberService : IBusinessMemberService
    {
        private readonly IBusinessMemberRepository _businessMemberRepository;
        private readonly IMapper _mapper;

        public BusinessMemberService(IBusinessMemberRepository businessMemberRepository
            ,IMapper mapper) 
        {
            _businessMemberRepository = businessMemberRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BusinessMemberDto>> GetAll()
        {
            var models = await _businessMemberRepository.GetAll();
            var result = _mapper.Map<IEnumerable<BusinessMemberDto>>(models);
            return result;
        }

        public async Task<BusinessMemberDto> GetById(int id)
        {
            var model = await _businessMemberRepository.GetById(id);
            var result = _mapper.Map<BusinessMemberDto>(model);
            return result;
        }

        public async Task<BusinessImg> GetImgById(int id)
        {
            return await _businessMemberRepository.GetImgById(id);
        }

        public async Task Modify(ModifyPwdParameterDto parameter)
        {
            await _businessMemberRepository.Modify(_mapper.Map<ModifyPwdParameterModel>(parameter));
        }
    }
}
