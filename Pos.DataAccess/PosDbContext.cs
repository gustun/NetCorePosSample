﻿using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Pos.Core.Interface;
using Pos.DataAccess.Entities;

namespace Pos.DataAccess
{
    public class PosDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PosDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Campaign> Campaigns { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderProduct> OrderProducts { get; set; }

        public override int SaveChanges()
        {
            Guid.TryParse(_httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(i => i.Type == ClaimTypes.Sid)?.Value, out var userId);

            var updatedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified);
            foreach (var entry in updatedEntities)
                if (entry.Entity is IStamp entity)
                {
                    if (userId != Guid.Empty)
                        entity.UpdatedUserId = userId;
                    entity.UpdatedDate = DateTime.Now;

                }

            var newEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);
            foreach (var entry in newEntities)
                if (entry.Entity is IStamp entity)
                {
                    if (userId != Guid.Empty)
                        entity.CreatedUserId = entity.UpdatedUserId = userId;
                    entity.CreatedDate = entity.UpdatedDate = DateTime.Now;
                }
            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .HasMany(x=>x.OrderProducts)
                .WithOne(x=>x.Order)
                .HasForeignKey(x=>x.OrderId)
                .HasPrincipalKey(x=>x.Id)
                .IsRequired();

            modelBuilder.Entity<OrderProduct>()
                .HasOne(sc => sc.Product)
                .WithMany(s => s.OrderProducts)
                .HasForeignKey(sc => sc.OrderId)
                .IsRequired();
        }
    }
}
