using MapsterMapper;
using prjMSIT145Final.Infrastructure.Models;
using prjMSIT145Final.Repository.Interfaces;
using prjMSIT145Final.Service.Interfaces;

namespace prjMSIT145Final.Service.Implements
{
    public class ServiceMailService : IServiceMailService
    {
        private readonly IServiceMailRepository _serviceMailRepo;
        private readonly IMapper _mapper;

        public ServiceMailService(IServiceMailRepository serviceMailRepository
            ,IMapper mapper)
        {
            _serviceMailRepo = serviceMailRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ServiceMailBox>> GetAll()
        {
            return await _serviceMailRepo.GetAll();
        }

        public async Task<ServiceMailBox> GetContentById(int id)
        {
            return await GetContentById(id);
        }

        public async Task<IEnumerable<ServiceMailBox>> GetUnreplied()
        {
            return await _serviceMailRepo.GetUnreplied();
        }

        public async Task SubmitReply(ServiceMailBox mail)
        {
            await _serviceMailRepo.SubmitReply(mail);
        }
    }
}
    