using prjMSIT145Final.Service.Dtos;
using prjMSIT145Final.Service.ParameterDtos;

namespace prjMSIT145Final.Service.Interfaces
{
    public interface INormalMemberService
    {
        Task<IEnumerable<NormalMemberDto>> GetAll();
        Task<NormalMemberDto> GetById(int id);

        Task Modify(ModifyPwdParameterDto parameter);
        Task<bool> CheckAccountInfo(ForgetPwdParameterDto parameter);


    }
}
