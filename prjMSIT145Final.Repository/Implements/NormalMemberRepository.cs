using Microsoft.EntityFrameworkCore;
using prjMSIT145Final.Infrastructure.Models;
using prjMSIT145Final.Repository.Interfaces;
using prjMSIT145Final.Repository.ParameterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjMSIT145Final.Repository.Implements
{
    public class NormalMemberRepository : INormalMemberRepository
    {
        private readonly ispanMsit145shibaContext _context;

        public NormalMemberRepository(ispanMsit145shibaContext context) 
        {
            _context = context;
        }

        public async Task<bool> CheckAccountInfo(ForgetPwdParameterModel parameter)
        {
            var user = await _context.NormalMembers.FirstOrDefaultAsync
                            (u => u.Phone == parameter.txtAccount && u.Email == parameter.txtEmail);

            return user != null;
        }

        public async Task<IEnumerable<NormalMember>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<NormalMember> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Modify(ModifyPwdParameterModel parameter)
        {
            var user = await Task.Run(()=>_context.NormalMembers.FirstOrDefault(t => t.Fid == parameter.Fid));
            if (!string.IsNullOrEmpty(parameter.Password) && (parameter.Password.Trim() == parameter.ConfirmPwd))
            {
                if (user != null)
                {
                    user.Password = parameter.Password;
                }
            }

            _context.SaveChanges();
        }
    }
}
