﻿using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WCPShared.Interfaces;
using WCPShared.Models.Entities;
using WCPShared.Models.Entities.UserModels;
using WCPShared.Models.Enums;

namespace WCPShared.Models
{
    public class TestDbContext : DbContext, IWcpDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Creator> Creators { get; set; }
        public DbSet<CreatorParticipation> CreatorParticipations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<StaticTemplate> StaticTemplates { get; set; }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .Property(e => e.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .Property(e => e.ProjectType)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .Property(e => e.Role)
                .HasConversion<string>();

            modelBuilder.Entity<Creator>()
                .Property(e => e.SubType)
                .HasConversion<string>();

            modelBuilder.Entity<CreatorParticipation>()
                .HasKey(e => new { e.OrderId, e.CreatorId });

            modelBuilder.Entity<CreatorParticipation>()
                .HasOne(e => e.Creator)
                .WithMany(e => e.Participations)
                .HasForeignKey(e => e.CreatorId);

            modelBuilder.Entity<CreatorParticipation>()
                .HasOne(e => e.Order)
                .WithMany(e => e.Participations)
                .HasForeignKey(e => e.OrderId);

            modelBuilder.Entity<Creator>()
                .HasMany(x => x.Languages)
                .WithMany(x => x.Speakers);

            modelBuilder.Entity<Order>()
                .HasMany(x => x.StaticTemplates)
                .WithMany(x => x.Orders);

            modelBuilder.Entity<Order>()
                .HasOne(x => x.Videographer);

            modelBuilder.Entity<Order>()
                .HasOne(x => x.Editor);

            //modelBuilder.Entity<Order>()
            //    .Property(x => x.ProjectTypeEnum)
            //    .HasConversion<string>();

            //modelBuilder.Entity<Order>()
            //    .Property(x => x.StatusEnum)
            //    .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .Property(x => x.Ideas)
                .HasConversion(new ValueConverter<List<Idea>, string>(
                    v => JsonConvert.SerializeObject(v.Select(x => x.Text)),
                    v => JsonConvert.DeserializeObject<List<string>>(v)!.Select(x => new Idea { Text = x }).ToList()));

            modelBuilder.Entity<Order>()
                .Property(x => x.Products)
                .HasConversion(new ValueConverter<List<Product>, string>(
                    v => JsonConvert.SerializeObject(v.Select(x => x.Link)),
                    v => JsonConvert.DeserializeObject<List<string>>(v)!.Select(x => new Product { Link = x }).ToList()));
        }
    }
}
