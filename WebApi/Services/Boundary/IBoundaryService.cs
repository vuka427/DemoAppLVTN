using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interface.IDomainServices
{
    public interface IBoundaryService
    {
        string GetAddress(int province,int district, int wards);
    }
}
