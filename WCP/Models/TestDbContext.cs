using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WCPShared.Models.UserModels;
using Newtonsoft.Json;
using WCPShared.Interfaces;

namespace WCPShared.Models
{
    public class TestDbContext : DbContext, IWcpDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Creator> Creators { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<StaticTemplate> StaticTemplates { get; set; }

        private readonly IConfiguration _configuration;

        public TestDbContext(DbContextOptions<TestDbContext> options, IConfiguration config) : base(options)
        {
            _configuration = config;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Creator>()
                .HasMany(x => x.Languages)
                .WithMany(x => x.Speakers);

            modelBuilder.Entity<Order>()
                .HasMany(x => x.StaticTemplates)
                .WithMany(x => x.Orders);

            modelBuilder.Entity<Order>()
                .HasMany(x => x.Creators)
                .WithMany(x => x.Orders);

            modelBuilder.Entity<Order>()
                .HasOne(x => x.Videographer);

            modelBuilder.Entity<Order>()
                .HasOne(x => x.Editor);

            modelBuilder.Entity<Order>()
                .Property(x => x.Ideas)
                .HasConversion(new ValueConverter<List<string>, string>(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<List<string>>(v)),
                    new ValueComparer<List<string>>(
                        (c1, c2) => c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToList()));

            modelBuilder.Entity<Order>()
                .Property(x => x.Products)
                .HasConversion(new ValueConverter<List<string>, string>(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<List<string>>(v)),
                    new ValueComparer<List<string>>(
                        (c1, c2) => c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToList()));
        }
    }
}
