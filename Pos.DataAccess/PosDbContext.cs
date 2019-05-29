using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Pos.Core.Enum;
using Pos.Core.Interface;
using Pos.DataAccess.Entities;
using Pos.Utility;

namespace Pos.DataAccess
{
    public class PosDbContext : DbContext
    {
        public PosDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }

        public override int SaveChanges()
        {
            var updatedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified);
            foreach (var entry in updatedEntities)
                if (entry.Entity is IStamp entity)
                    entity.UpdatedDate = DateTime.Now;

            var newEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);
            foreach (var entry in newEntities)
                if (entry.Entity is IStamp entity)
                    entity.CreatedDate = entity.UpdatedDate = DateTime.Now;
            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderProduct>()
            .HasOne(sc => sc.Order)
            .WithMany(s => s.OrderProducts)
            .HasForeignKey(sc => sc.OrderId)
            .IsRequired();

            modelBuilder.Entity<OrderProduct>()
                .HasOne(sc => sc.Product)
                .WithMany(s => s.OrderProducts)
                .HasForeignKey(sc => sc.OrderId)
                .IsRequired();
        }
    }
}
