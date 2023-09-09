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
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasKey(r => r.Id);
            builder.ToTable(nameof(Room));
            builder.Property(r => r.RoomNumber)
                .IsRequired();
            builder.Property(r=>r.Width)
                .IsRequired();
            builder.Property(r=>r.Height)
                .IsRequired();
            builder.Property(r=>r.Length)
                .IsRequired();
            builder.Property(r=>r.IsMezzanine)
                .HasDefaultValue(false)
                .IsRequired();
            builder.Property(r=>r.Price)
                .HasPrecision(10,0)
                .HasDefaultValue (0)
                .IsRequired();
            builder.Property(r => r.MaxMember)
                .HasDefaultValue(1)
                .IsRequired()
                .HasMaxLength(10);

            builder.HasMany<PostNew>(r => r.PostNews)
                .WithOne(p => p.Room)
                .HasForeignKey(p => p.RoomId);

            builder.HasMany<Device>(r => r.Devices)
                .WithOne(p => p.Room)
                .HasForeignKey(p => p.RoomId);
            builder.HasMany<RoomIndex>(r => r.RoomIndexs)
                 .WithOne(p => p.Room)
                 .HasForeignKey(p => p.RoomId);

            builder.HasMany<ImageRoom>(r => r.ImageRooms)
                .WithOne(p => p.Room)
                .HasForeignKey(p => p.RoomId);











        }
    }
}
