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
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable(nameof(Branch));
            builder.Property(b=>b.BranchName)
                .IsRequired()
                .HasColumnType("nvarchar")
                .HasMaxLength(256);
            builder.Property(b => b.Description)
               .IsRequired(false)
               .HasColumnType("nvarchar")
               .HasMaxLength(256);
            builder.Property(b => b.Address)
               .IsRequired()
               .HasColumnType("nvarchar")
               .HasMaxLength(256);
            builder.Property(b => b.ElectricityCosts)
                .HasPrecision(10,0)
                .HasDefaultValue(0)
                .IsRequired(false);
            builder.Property(b => b.WaterCosts)
                .HasPrecision(10, 0)
                .HasDefaultValue(0)
                .IsRequired(false);
            builder.Property(b => b.GarbageColletionFee)
                .HasPrecision(10, 0)
                .HasDefaultValue(0)
                .IsRequired(false);
            builder.Property(b => b.InternetCosts)
                .HasPrecision(10, 0)
                .HasDefaultValue(0)
                .IsRequired(false);
            builder.Property(b=>b.InternalRegulation)
                .IsRequired(false)
                .HasColumnType("text");


        }
    }
}
