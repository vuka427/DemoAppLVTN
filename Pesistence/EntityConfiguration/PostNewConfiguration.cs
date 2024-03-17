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
    public class PostNewConfiguration : IEntityTypeConfiguration<PostNew>
    {
        public void Configure(EntityTypeBuilder<PostNew> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable(nameof(PostNew));
            builder.Property(p => p.Title)
                .IsRequired()
                .HasColumnType("nvarchar")
                .HasMaxLength(256);
            builder.Property(p=>p.Content)
                .IsRequired()
                .HasColumnType("text")
                .HasMaxLength(500);

        }
    }
}
