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
    public class RoomIndexConfiguration : IEntityTypeConfiguration<RoomIndex>
    {
        public void Configure(EntityTypeBuilder<RoomIndex> builder)
        {
            throw new NotImplementedException();
        }
    }
}
