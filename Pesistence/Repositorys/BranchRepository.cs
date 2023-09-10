﻿using Domain.Entities;
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
    public class BranchRepository : EFRepository<Branch, int>, IBranchRepository
    {
        public BranchRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
