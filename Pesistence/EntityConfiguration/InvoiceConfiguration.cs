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
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable(nameof(Invoice));
            builder.Property(i=>i.InvoiceCode)
                .HasColumnType("varchar")
                .HasMaxLength(10);
			builder.Property(b => b.IsApproved)
			   .HasDefaultValue(false)
			   .IsRequired();
			builder.Property(i => i.OldElectricNumber)
                .IsRequired();
            builder.Property(i => i.OldWaterNumber)
                .IsRequired();
            builder.Property(i => i.NewElectricNumber)
                .IsRequired();
            builder.Property(i => i.NewWaterNumber)
                .IsRequired();

            builder.Property(b => b.ElectricityCosts)
                .HasPrecision(10, 0)
                .HasDefaultValue(0)
                .IsRequired();
            builder.Property(b => b.WaterCosts)
                .HasPrecision(10, 0)
                .HasDefaultValue(0)
                .IsRequired();
            builder.Property(b => b.GarbageColletionFee)
                .HasPrecision(10, 0)
                .HasDefaultValue(0)
                .IsRequired();
            builder.Property(b => b.InternetCosts)
                .HasPrecision(10, 0)
                .HasDefaultValue(0)
                .IsRequired();
            builder.Property(b => b.TotalPrice)
                .HasPrecision(10, 0)
                .HasDefaultValue(0)
                .IsRequired();

            builder.HasMany<ServiceItem>(i => i.ServiceItems)
                .WithOne(s => s.Invoice)
                .HasForeignKey(i => i.InvoiceId);


        }
    }
}
