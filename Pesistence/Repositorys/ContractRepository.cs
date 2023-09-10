using Domain.Entities;
using Domain.Interface;
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
    public class ContractRepository : EFRepository<Contract, int>, IContractRepository
    {
        public ContractRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
