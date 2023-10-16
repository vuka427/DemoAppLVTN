
using Application.Interface;
using Domain.Interface;
using Domain.IRepositorys;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pesistence.Common;
using Pesistence.EmailService;
using Pesistence.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pesistence
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IUnitOfWork, EFUnitOfWork>();
            //add repository
            services.AddTransient<IAdminRepository, AdminRepository>();
            services.AddTransient<IAreaRepository, AreaRepository>();
            services.AddTransient<IBranchRepository, BranchRepository>();
            services.AddTransient<IContractRepository, ContractRepository>();
            services.AddTransient<IDeviceRepository, DeviceRepository>();
            services.AddTransient<IEmailSendRepository, EmailSendRepository>();
            services.AddTransient<IImageRoomRepository, ImageRoomRepository>();
            services.AddTransient<IInvoiceRepository, InvoiceRepository>();
            services.AddTransient<ILandlordRepository, LandlordRepository>();
            services.AddTransient<IMemberRepository, MemberRepository>();
            services.AddTransient<IMessageRepository, MessageRepository>();
            services.AddTransient<IPostNewRepository, PostNewRepository>();
            services.AddTransient<IRoomRepository, RoomRepository>();
            services.AddTransient<IRoomIndexRepository, RoomIndexRepository>();
            services.AddTransient<IServiceRepository, ServiceRepository>();
            services.AddTransient<IServiceItemRepository, ServiceItemRepository>();
            services.AddTransient<ITenantRepository, TenantRepository>();

            //add email service
            services.AddTransient<IEmailService, EmailSender>();
        }
    } 
}
