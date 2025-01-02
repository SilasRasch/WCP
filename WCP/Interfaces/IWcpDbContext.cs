using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WCPShared.Models.Entities;
using WCPShared.Models.Entities.ProjectModels;
using WCPShared.Models.Entities.ProjectModels.Concepts;
using WCPShared.Models.Entities.UserModels;

namespace WCPShared.Interfaces
{
    public interface IWcpDbContext : IDisposable
    {
        DbSet<User> Users { get; set; }
        DbSet<Organization> Organizations { get; set; }
        DbSet<Brand> Brands { get; set; }
        DbSet<Creator> Creators { get; set; }
        DbSet<CreatorParticipation> CreatorParticipations { get; set; }
        DbSet<Language> Languages { get; set; }
        DbSet<StaticTemplate> StaticTemplates { get; set; }
        DbSet<Subscription> Subscriptions { get; set; }

        DbSet<Project> Projects { get; set; }
        //DbSet<UgcProject> UgcProjects { get; set; }
        //DbSet<StaticProject> StaticProjects { get; set; }
        //DbSet<PhotoProject> PhotoProjects { get; set; }
        public DbSet<Concept> Concepts { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<ChatMessage> Chats { get; set; }
        DbSet<Feedback> Feedbacks { get; set; }

        // Abstractions
        ChangeTracker ChangeTracker { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class;
        ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default(CancellationToken)) where TEntity : class;
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}
