﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Models.DTOs;
using WCPShared.Models.UserModels;

namespace WCPShared.Interfaces.DataServices
{
    public interface ICreatorService : IDatabaseService<Creator>
    {
        void Attach(Creator creator);
        Task<Creator?> UpdateObject(int id, CreatorDto creator);
    }
}
