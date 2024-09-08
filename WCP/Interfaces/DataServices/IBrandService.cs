using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Interfaces.DataServices.Utility;
using WCPShared.Models;
using WCPShared.Models.Views;

namespace WCPShared.Interfaces.DataServices
{
    public interface IBrandService : IDatabaseService<Brand>, IDtoExtensions<BrandDto, Brand>, IObjectViewService<Brand, BrandView>
    {
    }
}
