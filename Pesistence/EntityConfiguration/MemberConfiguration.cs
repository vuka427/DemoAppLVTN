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
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable(nameof(Member));
            builder.Property(t => t.FullName)
               .IsRequired()
               .HasColumnType("nvarchar")
               .HasMaxLength(256);
            builder.Property(m => m.Cccd)
               .IsRequired()
               .HasMaxLength(12);
            builder.Property(m => m.PlaceOfIssuance)
               .IsRequired()
               .HasColumnType("nvarchar")
               .HasMaxLength(256);
            builder.Property(m => m.PermanentAddress)
               .IsRequired()
               .HasColumnType("nvarchar")
               .HasMaxLength(256);
            builder.Property(m => m.Phone)
               .IsRequired()
               .HasMaxLength(10);
			builder.Property(m => m.IsPermanent)
			 .HasDefaultValue(false)
			 .IsRequired();
			builder.Property(m => m.Gender)
			 .HasDefaultValue(false)
			 .IsRequired();
            builder.Property(m => m.Job)
               .IsRequired()
			   .HasColumnType("nvarchar")
			   .HasMaxLength(256);
		}
    }
}
