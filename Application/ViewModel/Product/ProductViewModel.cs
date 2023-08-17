using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ViewModel.Product
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        
    }
}
