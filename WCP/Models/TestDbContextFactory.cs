using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Interfaces;

namespace WCPShared.Models
{
    public class TestDbContextFactory : IWcpDbContextFactory
    {
        private readonly IDbContextFactory<TestDbContext> _factory;

        public TestDbContextFactory(IDbContextFactory<TestDbContext> factory)
        {
            _factory = factory;
        }

        public IWcpDbContext CreateDbContext()
        {
            return _factory.CreateDbContext();
        }
    }
}
