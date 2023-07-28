using System;
using System.Collections.Generic;
using System.Text;
using Domain.Common;

namespace Domain.Entities
{
    public class Category : BaseEntity
    {
        public Category() { 
            Products = new HashSet<Product>();
        }

        public Category(string title, string description)
        {
            Title = title;
            Description = description;
            Products = new HashSet<Product>();
        }

        public string Title { get; set; }
        public string Description { get; set; }

        public ICollection<Product> Products { get; set; }

    }
}
