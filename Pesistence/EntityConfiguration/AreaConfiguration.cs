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
    public class AreaConfiguration : IEntityTypeConfiguration<Area>
    {
        public void Configure(EntityTypeBuilder<Area> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable(nameof(Area));
            builder.Property(a => a.AreaName)
                .IsRequired()
                .HasColumnType("nvarchar")
                .HasMaxLength(256);
            builder.Property(a=>a.Description)
                .IsRequired(false)
                .HasColumnType("nvarchar")
                .HasMaxLength(256);
            builder.HasMany<Room>(a => a.Rooms)
                .WithOne(r => r.Area)
                .HasForeignKey(r=>r.AreaId);

            

        }
    }
}
