using prjMSIT145Final.Infrastructure.Models;
using prjMSIT145Final.Repository.ParameterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjMSIT145Final.Repository.Interfaces
{
    public interface IBusinessMemberRepository
    {
        Task<IEnumerable<BusinessMember>> GetAll();
        Task<BusinessMember> GetById(int id);

        Task<BusinessImg> GetImgById(int id);

        Task Modify(ModifyPwdParameterModel parameter);

    }
}
