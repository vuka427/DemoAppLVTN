using Application.Interface.IRepositorys;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();

        }
    }
}
