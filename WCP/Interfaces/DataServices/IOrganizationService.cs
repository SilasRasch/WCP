﻿using WCPShared.Models;

namespace WCPShared.Interfaces.DataServices
{
    public interface IOrganizationService : IDatabaseService<Organization>
    {
        Task<Organization?> GetObject(int id, bool includeBrands = false);
        Task<IEnumerable<Organization>> GetAllObjects(bool includeBrands = false);
    }
}
