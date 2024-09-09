
using WCPShared.Interfaces.DataServices.Utility;
using WCPShared.Models;
using WCPShared.Models.DTOs;
using WCPShared.Models.Views;

namespace WCPShared.Interfaces.DataServices
{
    public interface IStaticTemplateService : IDatabaseService<StaticTemplate>, 
        IObjectViewService<StaticTemplate, StaticTemplateView>, 
        IDtoExtensions<StaticTemplateDto, StaticTemplate>
    {
    }
}
