using WCPShared.Interfaces.DataServices.Utility;
using WCPShared.Models;
using WCPShared.Models.DTOs;

namespace WCPShared.Interfaces.DataServices
{
    public interface IOrganizationService : IDatabaseService<Organization>, IDtoExtensions<OrganizationDto, Organization>
    {
        Task<Organization?> GetObject(int id, bool includeBrands = false);
        Task<List<Organization>> GetAllObjects(bool includeBrands = false);
    }
}
