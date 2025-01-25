using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCPShared.Interfaces
{
    public interface IWcpDbContextFactory
    {
        IWcpDbContext CreateDbContext();
    }
}
