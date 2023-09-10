using Domain.Entities;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.IRepositorys
{
    public interface IAdminRepository : IRepository<Admin,int>
    {
    }
}
