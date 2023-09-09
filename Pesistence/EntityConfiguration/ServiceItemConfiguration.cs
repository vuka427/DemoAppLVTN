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
    public class ServiceItemConfiguration : IEntityTypeConfiguration<ServiceItem>
    {
        public void Configure(EntityTypeBuilder<ServiceItem> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable(nameof(ServiceItem));
            builder.Property(s=>s.ServiceName)
                .IsRequired()
                .HasColumnType("nvarchar")
                .HasMaxLength(50);
            builder.Property(s=>s.Price)
                .IsRequired()
                .HasPrecision(10,0);
            builder.Property(s => s.Description)
               .IsRequired(false)
               .HasColumnType("nvarchar")
               .HasMaxLength(256);
        }
    }
}
