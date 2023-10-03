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
    public class ContractConfiguration : IEntityTypeConfiguration<Contract>
    {
        public void Configure(EntityTypeBuilder<Contract> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable(nameof(Contract));
            builder.Property(i => i.ContractCode)
               .HasColumnType("varchar")
               .HasMaxLength(10);
            builder.Property(t => t.A_Lessor)
               .IsRequired()
               .HasColumnType("nvarchar")
               .HasMaxLength(256);
            builder.Property(t => t.A_Cccd)
               .IsRequired()
               .HasMaxLength(12);
            builder.Property(t => t.A_PlaceOfIssuance)
               .IsRequired()
               .HasColumnType("nvarchar")
               .HasMaxLength(256);
            builder.Property(t => t.A_PermanentAddress)
               .IsRequired()
               .HasColumnType("nvarchar")
               .HasMaxLength(256);
            builder.Property(t => t.A_Phone)
               .IsRequired()
               .HasMaxLength(10);

            builder.Property(t => t.B_Lessee)
               .IsRequired()
               .HasColumnType("nvarchar")
               .HasMaxLength(256);
            builder.Property(t => t.B_Cccd)
               .IsRequired()
               .HasMaxLength(12);
            builder.Property(t => t.B_PlaceOfIssuance)
               .IsRequired()
               .HasColumnType("nvarchar")
               .HasMaxLength(256);
            builder.Property(t => t.B_PermanentAddress)
               .IsRequired()
               .HasColumnType("nvarchar")
               .HasMaxLength(256);
            builder.Property(t => t.B_Phone)
               .IsRequired()
               .HasMaxLength(10);

            builder.Property(b => b.RentalPrice)
                .HasPrecision(10, 0)
                .HasDefaultValue(0)
                .IsRequired();
            builder.Property(r => r.RoomNumber)
                .IsRequired();
            builder.Property(b => b.BranchName)
               .IsRequired()
               .HasColumnType("nvarchar")
               .HasMaxLength(256);
            builder.Property(b => b.BranchAddress)
               .IsRequired()
               .HasColumnType("nvarchar")
               .HasMaxLength(256);
            builder.Property(b => b.AreaName)
               .IsRequired()
               .HasColumnType("nvarchar")
               .HasMaxLength(256);
            builder.Property(r => r.Acreage)
                .IsRequired();
            builder.Property(r => r.IsMezzanine)
                .HasDefaultValue(false)
                .IsRequired();
            builder.Property(b => b.Deposit)
                .HasPrecision(10, 0)
                .HasDefaultValue(0)
                .IsRequired();

            builder.HasMany<Invoice>(c => c.Invoices)
                .WithOne(i => i.Contract)
                .HasForeignKey(i => i.ContractId)
                .OnDelete(DeleteBehavior.Cascade); 
            builder.HasMany<Member>(c=>c.Members)
                .WithOne(m=>m.Contract)
                .HasForeignKey(m=>m.ContractId)
                .OnDelete(DeleteBehavior.Cascade); 

            builder.HasOne<Room>(c => c.Room)
                .WithMany(r => r.Contracts)
                .HasForeignKey(r => r.RoomId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Tenant>(c=>c.Tenant)
                .WithMany(t=>t.Contracts)
                .HasForeignKey(t=>t.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Landlord>(c => c.Landlord)
                .WithMany(l => l.Contracts)
                .HasForeignKey(l => l.LandlordId)
                .OnDelete(DeleteBehavior.Cascade);




        }
    }
}
