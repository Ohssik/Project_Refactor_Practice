using prjMSIT145Final.Infrastructure.Models;
using prjMSIT145Final.Repository.ParameterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjMSIT145Final.Repository.Interfaces
{
    public interface INormalMemberRepository
    {
        Task<IEnumerable<NormalMember>> GetAll();
        Task<NormalMember> GetById(int id);

        Task Modify(ModifyPwdParameterModel parameter);
        Task<bool> CheckAccountInfo(ForgetPwdParameterModel parameter);


    }
}
