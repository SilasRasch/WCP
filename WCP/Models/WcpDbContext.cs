using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WCPShared.Models.BrandModels;
using WCPShared.Models.OrderModels;
using WCPShared.Models.UserModels;
using WCPShared.Models.UserModels.CreatorModels;

namespace WCPShared.Models
{
    public class WcpDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Creator> Creators { get; set; }
        public DbSet<Order> Orders { get; set; }

        private readonly IConfiguration _configuration;

        public WcpDbContext(DbContextOptions<WcpDbContext> options, IConfiguration config) : base(options)
        {
            _configuration = config;
        }
    }
}
