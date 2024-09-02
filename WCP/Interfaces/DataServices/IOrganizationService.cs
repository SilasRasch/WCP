using WCPShared.Models;
using WCPShared.Models.DTOs;

namespace WCPShared.Interfaces.DataServices
{
    public interface IOrganizationService : IDatabaseService<Organization>
    {
        Task<Organization?> GetObject(int id, bool includeBrands = false);
        Task<IEnumerable<Organization>> GetAllObjects(bool includeBrands = false);
        Task<Organization?> UpdateObject(int id, OrganizationDto organization);
    }
}
