using prjMSIT145Final.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjMSIT145Final.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetByNormalMemberId(int id);
        Task<IEnumerable<Order>> GetByBusinessMemberId(int id);

        Task<IEnumerable<dynamic>> Get(int id);
        
    }
}
