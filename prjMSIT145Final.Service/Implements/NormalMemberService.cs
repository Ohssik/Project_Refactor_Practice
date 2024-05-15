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
    public class NormalMemberService : INormalMemberService
    {
        public Task<bool> CheckAccountInfo(ForgetPwdParameterDto parameter)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<NormalMemberDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<NormalMemberDto> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Modify(ModifyPwdParameterDto parameter)
        {
            throw new NotImplementedException();
        }
    }
}
