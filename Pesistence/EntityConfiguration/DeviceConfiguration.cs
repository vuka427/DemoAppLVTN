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
    public class DeviceConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable(nameof(Device));
            builder.Property(d => d.DeviceName)
                .IsRequired()
                .HasColumnType("nvarchar")
                .HasMaxLength(256);
            builder.Property(d => d.Description)
               .IsRequired(false)
               .HasColumnType("nvarchar");
            builder.Property(d=>d.Quantity)
                .IsRequired()
                .HasMaxLength(10)
                .HasDefaultValue(1);
        }
    }
}
