using Domain.Entities;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interface.IRepositorys
{
    public interface IProductRepository : IRepository<Product, int>
    {
    }
}
