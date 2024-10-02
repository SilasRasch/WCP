using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Diagnostics.CodeAnalysis;
using WCPShared.Models.Entities;
using WCPShared.Models.Entities.UserModels;

namespace WCPShared.Interfaces
{
    public interface IWcpDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Organization> Organizations { get; set; }
        DbSet<Brand> Brands { get; set; }
        DbSet<Creator> Creators { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<Language> Languages { get; set; }
        DbSet<StaticTemplate> StaticTemplates { get; set; }

        // Abstractions
        ChangeTracker ChangeTracker { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class;
        ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default(CancellationToken)) where TEntity : class;
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}
