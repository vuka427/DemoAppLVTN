using Application.Implementation.DomainServices;
using Application.Interface.IDomainServices;
using Application.Interface.IRepositorys;
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
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IRoleService, RoleService>();
        }
    }
}
