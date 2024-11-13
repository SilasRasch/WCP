using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Interfaces;

namespace WCPShared.Models
{
    public class WcpDbContextFactory : IWcpDbContextFactory
    {
        private readonly IDbContextFactory<WcpDbContext> _factory;

        public WcpDbContextFactory(IDbContextFactory<WcpDbContext> factory)
        {
            _factory = factory;
        }

        public IWcpDbContext CreateDbContext()
        {
            return _factory.CreateDbContext();
        }
    }
}
