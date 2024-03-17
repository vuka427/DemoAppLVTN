using Domain.Entities;
using Domain.IRepositorys;
using Pesistence.AppDbContext;
using Pesistence.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pesistence.Repositorys
{
    public class LandlordRepository : EFRepository<Landlord, int>, ILandlordRepository
    {
        public LandlordRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
