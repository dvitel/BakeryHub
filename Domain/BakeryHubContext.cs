using BakeryHub.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    class BakeryHubContext : DbContext
    {
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Delivery> Delivery { get; set; }
        public DbSet<Product> Products { get; set; }
        public BakeryHubContext(DbContextOptions<BakeryHubContext> opts) : base(opts)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //API - Fluent API
            modelBuilder
                .Entity<Product>().ToTable("MyProducts")
                .HasMany(c => c.Delivery)
                .WithOne();
            base.OnModelCreating(modelBuilder);
        }
    }
}
