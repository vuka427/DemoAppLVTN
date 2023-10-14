using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interface
{
    public interface IUnitOfWork
    {
        void Commit();
        void Dispose();
    }
}
