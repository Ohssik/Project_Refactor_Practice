using prjMSIT145Final.Infrastructure.Models;
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
    public class BusinessMemberService : IBusinessMemberService
    {
        public Task<IEnumerable<BusinessMemberDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<BusinessMemberDto> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<BusinessImg> GetImgById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Modify(ModifyPwdParameterDto parameter)
        {
            throw new NotImplementedException();
        }
    }
}
