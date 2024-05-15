using prjMSIT145Final.Infrastructure.Models;
using prjMSIT145Final.Service.Dtos;
using prjMSIT145Final.Service.ParameterDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
