using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Models;

namespace WCPShared.Interfaces.DataServices
{
    public interface IBrandService : IDatabaseService<Brand>
    {
        Task<Brand?> UpdateObject(int id, BrandDto brand);
    }
}
