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
    public class LandlordConfiguration : IEntityTypeConfiguration<Landlord>
    {
        public void Configure(EntityTypeBuilder<Landlord> builder)
        {
            builder.HasKey(l => l.Id);
            builder.ToTable(nameof(Landlord));
            builder.Property(l => l.FullName)
                .IsRequired()
                .HasColumnType("nvarchar")
                .HasMaxLength(256);
            builder.Property(l => l.DateOfBirth)
                .IsRequired();
            builder.Property(l => l.Address)
                .IsRequired()
                .HasColumnType("nvarchar")
                .HasMaxLength(256);
            builder.Property(l => l.Ccccd)
                .IsRequired()
                .HasMaxLength(12);
            builder.Property(l => l.Phone)
                .IsRequired()
                .HasMaxLength(10);
            builder.Property(l => l.AvatarUrl)
                .IsRequired(false)
                .HasColumnType("nvarchar")
                .HasMaxLength(256);


            builder.HasOne<AppUser>(e => e.User)
               .WithOne()
               .HasForeignKey<Landlord>(a => a.UserId)
               .IsRequired()
               ;

            builder.HasMany<EmailSend>(l => l.EmailSends)
                .WithOne(e => e.Landlord)
                .HasForeignKey(e=>e.LandlordId);
            builder.HasMany<Message>(l => l.Messages)
                .WithOne(m => m.Landlord)
                .HasForeignKey(m => m.LandlordId);
            builder.HasMany<Branch>(l=>l.Branchs)
                .WithOne(b=>b.Landlord)
                .HasForeignKey (b=>b.LandlordId);

        }
    }
}
