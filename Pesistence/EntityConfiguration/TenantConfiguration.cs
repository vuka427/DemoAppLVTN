using Domain.Entities;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pesistence.EntityConfiguration
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.HasKey(t => t.Id);
            builder.ToTable(nameof(Tenant));
            builder.Property(t => t.FullName)
                .IsRequired()
                .HasColumnType("nvarchar")
                .HasMaxLength(256);
            builder.Property(t => t.DateOfBirth)
                .IsRequired();
            builder.Property(t=>t.Address)
                .IsRequired(false)
                .HasColumnType("nvarchar")
                .HasMaxLength(256);
            builder.Property(t=>t.Ccccd)
                .IsRequired(false)
                .HasMaxLength(12);
            builder.Property(t=>t.Phone)
                .IsRequired()
                .HasMaxLength(10);
            builder.Property(t => t.AvatarUrl)
                .IsRequired(false)
                .HasColumnType("nvarchar")
                .HasMaxLength(256);


            builder.HasOne<AppUser>(t => t.User)
               .WithOne()
               .HasForeignKey<Tenant>(t => t.UserId)
               .IsRequired()
               ;
            builder.HasMany<EmailSend>(t => t.EmailReceives)
                .WithOne(t => t.Tenant)
                .HasForeignKey(t => t.TenantId)
                .OnDelete(DeleteBehavior.Restrict)
                ;

            builder.HasMany<Message>(t => t.Messages)
                .WithOne(t => t.Tenant)
                .HasForeignKey(m => m.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
                
                

        }
    }
}
