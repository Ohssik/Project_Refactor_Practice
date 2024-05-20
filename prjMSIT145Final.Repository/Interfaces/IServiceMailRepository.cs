using prjMSIT145Final.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjMSIT145Final.Repository.Interfaces
{
    public interface IServiceMailRepository
    {
        Task<IEnumerable<ServiceMailBox>> GetAll();

        Task<IEnumerable<ServiceMailBox>> GetUnreplied();

        Task<ServiceMailBox> GetContentById(int id);

        Task SubmitReply(ServiceMailBox mail);

    }
}
