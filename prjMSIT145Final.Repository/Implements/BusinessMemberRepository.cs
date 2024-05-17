using Microsoft.EntityFrameworkCore;
using prjMSIT145Final.Infrastructure.Models;
using prjMSIT145Final.Repository.Interfaces;
using prjMSIT145Final.Repository.ParameterModels;

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
            var result = await _context.BusinessMembers.FirstOrDefaultAsync(member => member.Fid == id);
            return result ?? new BusinessMember();
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
