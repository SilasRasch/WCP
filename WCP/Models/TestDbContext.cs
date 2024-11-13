using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WCPShared.Interfaces;
using WCPShared.Models.Entities;
using WCPShared.Models.Entities.UserModels;
using WCPShared.Models.Enums;
using WCPShared.Models.Entities.ProjectModels;

namespace WCPShared.Models
{
    public class TestDbContext : DbContext, IWcpDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Creator> Creators { get; set; }
        public DbSet<CreatorParticipation> CreatorParticipations { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<StaticTemplate> StaticTemplates { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        // Project 2.0
        public DbSet<Project> Projects { get; set; }
        public DbSet<UgcProject> UgcProjects { get; set; }
        public DbSet<StaticProject> StaticProjects { get; set; }
        public DbSet<PhotoProject> PhotoProjects { get; set; }
        public DbSet<Product> Products { get; set; }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().UseTpcMappingStrategy();

            modelBuilder.Entity<Project>()
                .HasOne(x => x.Product)
                .WithMany(x => x.Projects);

            modelBuilder.Entity<Project>()
                .Property(x => x.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Subscription>()
                .Property(e => e.Type)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .Property(e => e.Role)
                .HasConversion<string>();

            modelBuilder.Entity<Creator>()
                .Property(e => e.SubType)
                .HasConversion<string>();

            modelBuilder.Entity<Creator>()
                .HasMany(x => x.Languages)
                .WithMany(x => x.Speakers);

            modelBuilder.Entity<CreatorParticipation>()
                .HasKey(e => new { e.ProjectId, e.CreatorId });

            modelBuilder.Entity<CreatorParticipation>()
                .HasOne(e => e.Creator)
                .WithMany(e => e.Participations)
                .HasForeignKey(e => e.CreatorId);

            modelBuilder.Entity<CreatorParticipation>()
                .HasOne(e => e.Project)
                .WithMany(e => e.Participations)
                .HasForeignKey(e => e.ProjectId);

            modelBuilder.Entity<StaticProject>()
                .HasMany(x => x.StaticTemplates)
                .WithMany(x => x.Projects);

            modelBuilder.Entity<CreatorProject>()
                .Property(x => x.CreatorAge)
                .HasConversion(new ValueConverter<int[], string>(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<int[]>(v)!));

            modelBuilder.Entity<CreatorProject>()
                .Property(x => x.CreatorBudget)
                .HasConversion(new ValueConverter<long[], string>(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<long[]>(v)!));

            modelBuilder.Entity<CreatorProject>()
                .Property(x => x.Tags)
                .HasConversion(new ValueConverter<List<string>, string>(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<List<string>>(v)!));
        }
    }
}
