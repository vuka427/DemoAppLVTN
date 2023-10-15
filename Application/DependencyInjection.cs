using Application.Implementation.ApplicationServices;
using Application.Interface.ApplicationServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application
{
    public static class DependencyInjection
    {
        public static void AddApplicationCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<ILandlordService, LandlordService>();
            services.AddTransient<ITenantService, TenantService>();
            services.AddTransient<IBranchService, BranchService>();
            services.AddTransient<IRoomService, RoomService>();
            services.AddTransient<IContractService, ContractService>();

        }
    }
}
