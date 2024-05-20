using Microsoft.EntityFrameworkCore;
using prjMSIT145Final.Infrastructure.Models;
using prjMSIT145Final.Repository.Interfaces;

namespace prjMSIT145Final.Repository.Implements
{
    public class ServiceMailRepository : IServiceMailRepository
    {
        private readonly ispanMsit145shibaContext _context;

        public ServiceMailRepository(ispanMsit145shibaContext context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<ServiceMailBox>> GetAll()
        {
            var result = await Task.Run(() => _context.ServiceMailBoxes.Select(m => m));
            return result ?? Enumerable.Empty<ServiceMailBox>();
        }

        public async Task<ServiceMailBox> GetContentById(int id)
        {
            var result = await _context.ServiceMailBoxes.FirstOrDefaultAsync(m => m.Fid == id);
            return result ?? new ServiceMailBox();
        }

        public async Task<IEnumerable<ServiceMailBox>> GetUnreplied()
        {
            var result = await Task.Run(()=>_context.ServiceMailBoxes.Where(m => string.IsNullOrEmpty(m.Reply)));
            return result ?? Enumerable.Empty<ServiceMailBox>();
        }

        public async Task SubmitReply(ServiceMailBox mail)
        {
            var result = await _context.ServiceMailBoxes.FirstOrDefaultAsync(m => m.Fid == mail.Fid);

            if(result != null)
            {
                result.Reply = mail.Reply;
                result.ReadTime = mail.ReadTime;
                await _context.SaveChangesAsync();
            }
        }
    }
}
