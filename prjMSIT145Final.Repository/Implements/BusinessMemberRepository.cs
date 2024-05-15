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
    public class BusinessMemberRepository : IBusinessMemberRepository
    {
        private readonly ispanMsit145shibaContext _context;

        public BusinessMemberRepository(ispanMsit145shibaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BusinessMember>> GetAll()
        {
            var result = await Task.Run(() => {
               return _context.BusinessMembers.Select(item => item);
            });

            return result.ToList();
        }

        public async Task<BusinessMember> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<BusinessImg> GetImgById(int id)
        {
            var result = await Task.Run(() => 
                _context.BusinessImgs.FirstOrDefault(bl => bl.BFid == id)
            );

            return result ?? new BusinessImg();
        }

        public async Task Modify(ModifyPwdParameterModel parameter)
        {
            var user = await Task.Run(() => _context.BusinessMembers.FirstOrDefault(t => t.Fid == parameter.Fid));
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
