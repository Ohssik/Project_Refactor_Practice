using MapsterMapper;
using prjMSIT145Final.Repository.Interfaces;
using prjMSIT145Final.Repository.ParameterModels;
using prjMSIT145Final.Service.Dtos;
using prjMSIT145Final.Service.Interfaces;
using prjMSIT145Final.Service.ParameterDtos;

namespace prjMSIT145Final.Service.Implements
{
    public class NormalMemberService : INormalMemberService
    {
        private readonly INormalMemberRepository _normalMemberRepository;
        private readonly IMapper _mapper;

        public NormalMemberService(INormalMemberRepository normalMemberRepository
            ,IMapper mapper) 
        {
            _normalMemberRepository = normalMemberRepository;
            _mapper = mapper;
        }

        public async Task<bool> CheckAccountInfo(ForgetPwdParameterDto parameter)
        {
            var parameterModel = _mapper.Map<ForgetPwdParameterModel>(parameter);
            return await _normalMemberRepository.CheckAccountInfo(parameterModel);
        }

        public async Task<IEnumerable<NormalMemberDto>> GetAll()
        {
            var models = await _normalMemberRepository.GetAll();
            var result = _mapper.Map<IEnumerable<NormalMemberDto>>(models);
            return result;
        }

        public async Task<NormalMemberDto> GetById(int id)
        {
            var model = await _normalMemberRepository.GetById(id);
            var result = _mapper.Map<NormalMemberDto>(model);
            return result;
        }

        public async Task Modify(ModifyPwdParameterDto parameter)
        {
            var parameterModel = _mapper.Map<ModifyPwdParameterModel>(parameter);
            await _normalMemberRepository.Modify(parameterModel);
        }
    }
}
