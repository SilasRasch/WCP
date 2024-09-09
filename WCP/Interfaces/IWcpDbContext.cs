﻿using Microsoft.EntityFrameworkCore;
using WCPShared.Models.UserModels;
using WCPShared.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

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

        ChangeTracker ChangeTracker { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class;
        ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default(CancellationToken)) where TEntity : class;
    }
}
