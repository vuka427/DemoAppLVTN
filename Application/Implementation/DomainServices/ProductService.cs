using Application.Interface.IDomainServices;
using Application.Interface.IRepositorys;
using Application.ViewModel.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Implementation.DomainServices
{
    public class ProductService : IProductService
    {
        private IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public ProductViewModel GetById(int id)
        {
            var product = _productRepository.FindById(id, x => x.Category);

            return new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CategoryId = product.CategoryId,
                
            };

        }
    }
}
