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
                .IsRequired();
            builder.Property(b => b.WaterCosts)
                .HasPrecision(10, 0)
                .HasDefaultValue(0)
                .IsRequired();
           
            builder.Property(b=>b.InternalRegulation)
                .IsRequired()
                .HasColumnType("text");

            builder.HasMany<Area>(b => b.Areas)
                .WithOne(a => a.Branch)
                .HasForeignKey(b => b.BranchId);
            builder.HasMany<Service>(b => b.Services)
                .WithOne(s => s.Branch)
                .HasForeignKey(s=>s.BranchId);


        }
    }
}
