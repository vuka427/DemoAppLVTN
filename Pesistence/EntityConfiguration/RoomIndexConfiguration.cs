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
    public class RoomIndexConfiguration : IEntityTypeConfiguration<RoomIndex>
    {
        public void Configure(EntityTypeBuilder<RoomIndex> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable(nameof(RoomIndex));
            builder.Property(i => i.ElectricNumber)
                .IsRequired()
                .HasDefaultValue(0);
            builder.Property(i=>i.WaterNumber)
                .IsRequired()
                .HasDefaultValue(0);
            

        }
    }
}
