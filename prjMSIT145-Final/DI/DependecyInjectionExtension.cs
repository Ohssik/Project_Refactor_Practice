using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using prjMSIT145Final.Helpers;
using prjMSIT145Final.Infrastructure.Models;
using prjMSIT145Final.Repository.Implements;
using prjMSIT145Final.Repository.Interfaces;
using prjMSIT145Final.Service.Implements;
using prjMSIT145Final.Service.Interfaces;

namespace prjMSIT145Final.DI
{
    public static class DependecyInjectionExtension
    {
        public static void AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ispanMsit145shibaContext>(
                 options => options.UseSqlServer(
                 configuration.GetConnectionString("ispanMsit145shibaconnection" /*"localconnection"*/)
                ));
            services.AddScoped<IMapper, ServiceMapper>();
            services.AddScoped<ISendMailHelper, SendMailHelper>();
            services.AddScoped<IUploadImgHelper, UploadImgHelper>();

            #region Services
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IBusinessMemberService, BusinessMemberService>();
            services.AddScoped<INormalMemberService, NormalMemberService>();
            services.AddScoped<IOrderService, OrderService>();
            #endregion

            #region Repositories
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IBusinessMemberRepository, BusinessMemberRepository>();
            services.AddScoped<INormalMemberRepository, NormalMemberRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            #endregion
        }
    }
}
