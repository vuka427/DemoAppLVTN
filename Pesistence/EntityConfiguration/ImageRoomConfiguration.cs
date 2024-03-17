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
    public class ImageRoomConfiguration : IEntityTypeConfiguration<ImageRoom>
    {
        public void Configure(EntityTypeBuilder<ImageRoom> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable(nameof(ImageRoom));
            builder.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("varchar");

        }
    }
}
