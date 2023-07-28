using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Pesistence.EntityConfiguration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {

            builder.ToTable("SanPham");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name)
                    .HasColumnName("name")
                    .HasColumnType("nvarchar")
                    .HasMaxLength(50);
            builder.Property(x => x.Description).HasMaxLength(50);

            builder.HasOne<Category>(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);
        }
    }
}
