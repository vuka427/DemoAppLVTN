using Application.Interface.IRepositorys;
using Application.ViewModel.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interface.IDomainServices
{
    public interface IProductService
    {


         ProductViewModel GetById(int id);
       


    }
}
