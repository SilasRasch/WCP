using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using WCPShared.Interfaces;
using WCPShared.Models.Entities;
using WCPShared.Models.Entities.ProjectModels;
using WCPShared.Models.Entities.ProjectModels.Concepts;
using WCPShared.Models.Entities.UserModels;
using WCPShared.Models.Enums;

namespace WCPShared.Models
{
    public class V2Context : DbContext, IWcpDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Creator> Creators { get; set; }
        public DbSet<CreatorParticipation> CreatorParticipations { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<StaticTemplate> StaticTemplates { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Project> Projects { get; set; }
        public virtual DbSet<Concept> Concepts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ChatMessage> Chats { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        public V2Context(DbContextOptions<V2Context> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Concept>()
                .HasDiscriminator<ProjectType>("Type")
                .HasValue<UgcConcept>(ProjectType.UGC)
                .HasValue<StaticConcept>(ProjectType.Statics)
                .HasValue<PhotoConcept>(ProjectType.Photos);

            modelBuilder.Entity<Concept>()
                .HasOne(x => x.Product)
                .WithMany(x => x.Concepts);

            modelBuilder.Entity<Project>()
                .Property(x => x.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Project>()
                .Property(x => x.Type)
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

            modelBuilder.Entity<Concept>()
                .Property(x => x.Formats)
                .HasConversion(new ValueConverter<List<string>, string>(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<List<string>>(v)!));

            modelBuilder.Entity<Project>()
                .Property(x => x.Files)
                .HasConversion(new ValueConverter<List<string>, string>(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<List<string>>(v)!));

            modelBuilder.Entity<Creator>()
                .Property(x => x.PriceEstimate)
                .HasConversion(new ValueConverter<long[], string>(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<long[]>(v)!));

            modelBuilder.Entity<Creator>()
                .Property(x => x.Tags)
                .HasConversion(new ValueConverter<IEnumerable<string>, string>(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<IEnumerable<string>>(v)!));
        }
    }
}
