using prjMSIT145Final.Infrastructure.Models;
using prjMSIT145Final.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjMSIT145Final.Service.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetByNormalMemberId(int id);
        Task<IEnumerable<OrderDto>> GetByBusinessMemberId(int id);

        Task<IEnumerable<dynamic>> Get(int id);

    }
}
