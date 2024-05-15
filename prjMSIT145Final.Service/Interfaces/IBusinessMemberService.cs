using prjMSIT145Final.Infrastructure.Models;
using prjMSIT145Final.Service.Dtos;
using prjMSIT145Final.Service.ParameterDtos;

namespace prjMSIT145Final.Service.Interfaces
{
    public interface IBusinessMemberService
    {
        Task<IEnumerable<BusinessMemberDto>> GetAll();
        Task<BusinessMemberDto> GetById(int id);
        Task<BusinessImg> GetImgById(int id);

        Task Modify(ModifyPwdParameterDto parameter);

    }
}
