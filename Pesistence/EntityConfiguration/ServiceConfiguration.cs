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
    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable(nameof(Service));
            builder.Property(s => s.ServiceName)
                .IsRequired()
                .HasColumnType("nvarchar")
                .HasMaxLength(50);
            builder.Property(s => s.Price)
                .IsRequired()
                .HasPrecision(10, 0);
            builder.Property(s => s.Description)
               .IsRequired(false)
               .HasColumnType("nvarchar")
               .HasMaxLength(256);
        }
    }
}
