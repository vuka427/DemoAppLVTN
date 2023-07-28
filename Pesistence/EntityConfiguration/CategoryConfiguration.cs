using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pesistence.EntityConfiguration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
           builder.HasKey(x => x.Id);
            builder.Property(x=>x.Title)
                .HasColumnType("text")
                .IsRequired();
            builder.Property(x=> x.Description)
                .HasColumnType("nvarchar")
                .HasMaxLength(256);

     
        }
    }
}
