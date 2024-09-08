using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Interfaces.DataServices.Utility;
using WCPShared.Models.DTOs;
using WCPShared.Models.UserModels;
using WCPShared.Models.Views;

namespace WCPShared.Interfaces.DataServices
{
    public interface ICreatorService : IDatabaseService<Creator>, IDtoExtensions<CreatorDto, Creator>, IObjectViewService<Creator, CreatorView>
    {
    }
}
