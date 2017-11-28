using BakeryHub.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryHub.Domain
{
    public class BakeryHubContext : DbContext
    {
        public DbSet<CountryState> States { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierAddress> SupplierAddress { get; set; }
        public DbSet<SupplierContact> SupplierContacts { get; set; }
        public DbSet<CustomerContactSubscription> SupplierSubscriptions { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAddress> CustomerAddress { get; set; }
        public DbSet<CustomerContact> CustomerContacts { get; set; }
        public DbSet<CustomerContactSubscription> CustomerSubscriptions { get; set; }
        public DbSet<CartItem> Carts { get; set; }    
        public DbSet<SupplierReview> SupplierReviews { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<Delivery> Delivery { get; set; }
        public DbSet<DeliverySite> DeliverySites { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<CardPaymentMethod> PaymentMethods { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public BakeryHubContext(DbContextOptions<BakeryHubContext> opts) : base(opts)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //States
            modelBuilder
                .Entity<CountryState>()
                .HasKey(s => s.Code);
            modelBuilder
                .Entity<CountryState>()
                .Property(s => s.Code)
                .IsRequired()
                .HasMaxLength(2);
            modelBuilder
                .Entity<CountryState>()
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);
            //User entity
            modelBuilder
                .Entity<User>()
                .Property(u => u.Login)
                .IsRequired(true)
                .HasMaxLength(100);//nvarchar(100)
            modelBuilder
                .Entity<User>()
                .Property(u => u.Password)
                .IsRequired(true)
                .HasMaxLength(100);
            modelBuilder
                .Entity<User>()
                .HasIndex(u => u.Login);

            //Supplier entity
            modelBuilder
                .Entity<Supplier>()
                .Property(s => s.Name)
                .IsRequired(true)
                .HasMaxLength(100);
            modelBuilder
                .Entity<Supplier>()
                .Property(s => s.Description)
                .IsRequired(true)
                .HasMaxLength(400);
            modelBuilder
                .Entity<Supplier>()
                .HasOne(s => s.User)
                .WithOne()
                .HasForeignKey<Supplier>(s => s.Id);
                //.OnDelete(DeleteBehavior.)
            modelBuilder
                .Entity<Supplier>()
                .HasMany(s => s.Addresses)
                .WithOne()
                .HasForeignKey(a => a.SupplierId);
            modelBuilder
                .Entity<Supplier>()
                .HasMany(s => s.Contacts)
                .WithOne()
                .HasForeignKey(c => c.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder
                .Entity<Supplier>()
                .HasMany(s => s.Products)
                .WithOne()
                .HasForeignKey(s => s.SupplierId);
            modelBuilder
                .Entity<Supplier>()
                .HasMany(s => s.Reviews)
                .WithOne(r => r.Supplier)
                .HasForeignKey(s => s.SupplierId)
                .HasPrincipalKey(s => s.Id);
            modelBuilder
                .Entity<SupplierAddress>()
                .HasKey(a => new { a.SupplierId, a.AddressId });
            modelBuilder
                .Entity<SupplierAddress>()
                .HasOne(a => a.State)
                .WithOne()
                .HasForeignKey<SupplierAddress>(a => a.StateId);
            modelBuilder
                .Entity<SupplierAddress>(e =>
                {
                    e.Property(a => a.City)
                        .IsRequired(true)
                        .HasMaxLength(100);
                    e.Property(a => a.Street)
                        .IsRequired(true)
                        .HasMaxLength(400);
                    e.Property(a => a.Zip)
                        .HasMaxLength(30);
                });
            modelBuilder
                .Entity<SupplierContact>()
                .Property(c => c.Address)
                .IsRequired(true)
                .HasMaxLength(255);
            modelBuilder
                .Entity<SupplierContact>()
                .HasKey(c => new { c.SupplierId, c.ContactId });
            modelBuilder
                .Entity<SupplierContact>()
                .HasMany(c => c.Subscriptions)
                .WithOne()
                .HasForeignKey(s => new { s.SupplierId, s.ContactId })
                .HasPrincipalKey(c => new { c.SupplierId, c.ContactId });
            modelBuilder
                .Entity<SupplierContactSubscription>()
                .HasIndex(s => new { s.SupplierId, s.ContactId });
            modelBuilder
                .Entity<SupplierReview>()
                .HasKey(r => new { r.SupplierId, r.Date, r.CustomerId });
            modelBuilder
                .Entity<SupplierReview>()
                .Property(p => p.Feedback)
                .IsRequired()
                .HasMaxLength(400);

            modelBuilder
                .Entity<Product>()
                .HasKey(p => new { p.SupplierId, p.ProductId });
            modelBuilder
                .Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products);
            modelBuilder
                .Entity<Product>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(255);
            modelBuilder
                .Entity<Product>()
                .Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(400);
            modelBuilder
                .Entity<Product>()
                .HasMany(p => p.Images)
                .WithOne()
                .HasForeignKey(i => new { i.SupplierId, i.ProductId });
            modelBuilder
                .Entity<Product>()
                .HasMany(p => p.Reviews)
                .WithOne(p => p.Product)
                .HasForeignKey(r => new { r.CustomerId, r.ProductId })
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder
                .Entity<ProductCategory>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(255);
            modelBuilder
                .Entity<ProductCategory>()
                .Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(400);
            modelBuilder
                .Entity<ProductReview>()
                .HasKey(r => new { r.ProductId, r.Date, r.CustomerId });
            modelBuilder
                .Entity<ProductReview>()
                .Property(r => r.Feedback)
                .IsRequired()
                .HasMaxLength(400);

            modelBuilder
                .Entity<Customer>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(255);
            modelBuilder
                .Entity<Customer>()
                .HasOne(c => c.User)
                .WithOne()
                .HasForeignKey<Customer>(c => c.Id);
            modelBuilder
                .Entity<Customer>()
                .HasMany(c => c.CartItems)
                .WithOne()
                .HasForeignKey(i => i.CustomerId);

            modelBuilder
                .Entity<Customer>()
                .HasMany(c => c.SupplierReviews)
                .WithOne(r => r.Customer)
                .HasForeignKey(r => r.CustomerId)
                .HasPrincipalKey(c => c.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Customer>()
                .HasMany(c => c.ProductReviews)
                .WithOne()
                .HasForeignKey(r => r.CustomerId);

            modelBuilder
                .Entity<Customer>()
                .HasMany(c => c.Orders)
                .WithOne(c => c.Customer);

            modelBuilder
                .Entity<Customer>()
                .HasMany(c => c.Payments)
                .WithOne(c => c.Customer);

            modelBuilder
                .Entity<Customer>()
                .HasMany(c => c.Addresses)
                .WithOne()
                .HasForeignKey(a => a.CustomerId);

            modelBuilder
                .Entity<Customer>()
                .HasMany(c => c.Contacts)
                .WithOne()
                .HasForeignKey(a => a.CustomerId);

            modelBuilder
                .Entity<CustomerContact>()
                .HasKey(c => new { c.CustomerId, c.ContactId });

            modelBuilder
                .Entity<CustomerContact>()
                .HasMany(c => c.Subscriptions)
                .WithOne()
                .HasForeignKey(s => new { s.CustomerId, s.ContactId });

            modelBuilder
                .Entity<CustomerAddress>()
                .HasKey(a => new { a.CustomerId, a.AddressId });

            modelBuilder
                .Entity<CustomerAddress>()
                .HasOne(a => a.State)
                .WithOne()
                .HasForeignKey<CustomerAddress>(a => a.StateId);

            modelBuilder
                .Entity<CustomerAddress>(e =>
                {
                    e.Property(a => a.City)
                        .IsRequired(true)
                        .HasMaxLength(100);
                    e.Property(a => a.Street)
                        .IsRequired(true)
                        .HasMaxLength(400);
                    e.Property(a => a.Zip)
                        .HasMaxLength(30);
                });

            modelBuilder
                .Entity<CartItem>()
                .HasKey(i => new { i.CustomerId, i.ItemId });

            modelBuilder
                .Entity<CartItem>()
                .HasOne(c => c.Product)
                .WithMany()
                .HasForeignKey(i => new { i.SupplierId, i.ProductId })
                .HasPrincipalKey(c => new { c.SupplierId, c.ProductId })
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Order>()
                .HasKey(o => new { o.CustomerId, o.OrderId });

            modelBuilder
                .Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne()
                .HasForeignKey(oi => new { oi.CustomerId, oi.OrderId });

            modelBuilder
                .Entity<OrderItem>()
                .HasKey(oi => new { oi.CustomerId, oi.OrderId, oi.ProductId });

            modelBuilder
                .Entity<Delivery>()
                .HasKey(d => new { d.OrderId, d.SupplierId, d.CustomerId });

            modelBuilder
                .Entity<Delivery>()
                .HasOne(d => d.Order)
                .WithMany()
                .HasForeignKey(d => new { d.CustomerId, d.OrderId })
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Delivery>()
                .HasOne(d => d.Supplier)
                .WithMany()
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Delivery>()
                .HasOne(d => d.Customer)
                .WithMany()
                .HasForeignKey(d => d.CustomerId);

            modelBuilder
                .Entity<Delivery>()
                .HasOne(d => d.DeliverySite)
                .WithMany()
                .HasForeignKey(d => d.DeliverySiteId);

            modelBuilder
                .Entity<DeliverySite>()
                .Property(d => d.Name)
                .IsRequired(true)
                .HasMaxLength(100);

            modelBuilder
                .Entity<DeliverySite>()
                .Property(d => d.Description)
                .IsRequired(true)
                .HasMaxLength(400);

            modelBuilder
                .Entity<CardPaymentMethod>()
                .HasKey(pm => new { pm.CustomerId, pm.PaymentMethodId });

            modelBuilder
                .Entity<CardPaymentMethod>(e =>
                {
                    e.Property(c => c.NameOnCard)
                        .IsRequired(true)
                        .HasMaxLength(100);
                    e.Property(c => c.CardNumber)
                        .IsRequired(true)
                        .HasMaxLength(16);
                    e.Property(a => a.City)
                        .IsRequired(true)
                        .HasMaxLength(100);
                    e.Property(a => a.Street)
                        .IsRequired(true)
                        .HasMaxLength(400);
                    e.Property(a => a.Zip)
                        .HasMaxLength(30);
                    e.Property(a => a.Country)
                        .HasMaxLength(100);
                });

            modelBuilder.Entity<Payment>()
                .HasKey(p => new { p.CustomerId, p.PaymentId });

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithOne()
                .HasForeignKey<Payment>(p => new { p.CustomerId, p.OrderId })
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CustomerContactSubscription>()
                .HasKey(s => new { s.CustomerId, s.ContactId, s.Purpose });

            modelBuilder.Entity<SupplierContactSubscription>()
                .HasKey(s => new { s.SupplierId, s.ContactId, s.Purpose });

            modelBuilder.Entity<ProductImage>()
                .HasKey(i => new { i.SupplierId, i.ProductId, i.ImageId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
