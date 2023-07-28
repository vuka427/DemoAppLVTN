using System;
using System.Collections.Generic;
using System.Text;
using Domain.Common;

namespace Domain.Entities
{
    public class Product : BaseEntity
    {
        public Product() { }

        public Product(string name, string description, Category category)
        {
            Name = name;
            Description = description;
            Category = category;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
