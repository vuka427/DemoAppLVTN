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
    public class AdminConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(e=>e.FullName)
                .IsRequired()
                .HasColumnType("nvarchar")
                .HasMaxLength(256);
            builder.Property(e=>e.AvatarUrl)
                .IsRequired(false)
                .HasColumnType("nvarchar")
                .HasMaxLength(256);

            builder.HasOne<AppUser>(e => e.User) 
                .WithOne()
                .HasForeignKey<Admin>(a => a.UserId)
                .IsRequired()
                ;
                
            
        }
    }
}
