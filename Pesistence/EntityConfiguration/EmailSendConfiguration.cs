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
    public class EmailSendConfiguration : IEntityTypeConfiguration<EmailSend>
    {
        public void Configure(EntityTypeBuilder<EmailSend> builder)
        {
            builder.HasKey(e => e.Id);
            builder.ToTable(nameof(EmailSend));
            builder.Property(e=>e.EmailSender)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(320);

            builder.Property(e=>e.EmailReceiver)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(320);
            builder.Property(e=>e.Title)
                .IsRequired()
                .HasColumnType("nvarchar")
                .HasMaxLength(256);
            builder.Property(e=>e.Content)
                .IsRequired()
                .HasDefaultValue("")
                .HasColumnType("nvarchar(max)");
            builder.Property(l => l.ReceiverName)
               .IsRequired()
               .HasDefaultValue("")
               .HasColumnType("nvarchar")
               .HasMaxLength(256); 
            builder.Property(l => l.RoomName)
                .IsRequired()
                .HasDefaultValue("")
                .HasColumnType("nvarchar")
                .HasMaxLength(256);



        }
    }
}
