using prjMSIT145Final.Infrastructure.Models;

namespace prjMSIT145Final.Service.Interfaces
{
    public interface IServiceMailService
    {
        Task<IEnumerable<ServiceMailBox>> GetAll();

        Task<IEnumerable<ServiceMailBox>> GetUnreplied();

        Task<ServiceMailBox> GetContentById(int id);

        Task SubmitReply(ServiceMailBox mail);
    }
}
