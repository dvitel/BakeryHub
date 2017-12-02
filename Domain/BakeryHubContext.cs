using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BakeryHub.Domain
{
    public class BakeryHubContext : DbContext
    {
        public DbSet<CountryState> States { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Session> Session { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<DeliverySite> DeliverySites { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<OrderSubscription> OrderSubscription { get; set; }        
        public DbSet<ReportSubscription> ReportSubscription { get; set; }
        public DbSet<Delivery> Delivery { get; set; }
        public DbSet<Handshake> Handshake { get; set; }
        public DbSet<HandshakeComment> HandshakeComments { get; set; }
        public DbSet<NotificationLog> NotificationLog { get; set; }             
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderPaymentSensitiveInfo> OrderPaymentSensitiveInfo { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<CardPaymentMethod> CardPaymentMethods { get; set; }
        public DbSet<PayPalPaymentMethod> PayPalPaymentMethods { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Review> Reviews { get; set; }
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
                .IsUnicode(false)
                .HasMaxLength(2);
            modelBuilder
                .Entity<CountryState>()
                .Property(s => s.Name)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(100);
            //Countries 
            //States
            modelBuilder
                .Entity<Country>()
                .HasKey(s => s.Code);
            modelBuilder
                .Entity<Country>()
                .Property(s => s.Code)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(2);
            modelBuilder
                .Entity<Country>()
                .Property(s => s.Name)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(100);

            modelBuilder
                .Entity<Session>()
                .Property(s => s.IP)
                .IsRequired()
                .HasMaxLength(39);

            modelBuilder
                .Entity<Session>()
                .Property(s => s.UserAgent)
                .IsRequired()
                .HasMaxLength(400);

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
                .HasMaxLength(200);
            modelBuilder
                .Entity<User>()
                .HasIndex(u => u.Login)
                .IsUnique();
            modelBuilder
                .Entity<User>()
                .HasOne(u => u.Supplier)
                .WithOne(s => s.User)
                .HasForeignKey<Supplier>(s => s.Id)
                .HasPrincipalKey<User>(u => u.Id);
            modelBuilder
                .Entity<User>()
                .HasOne(u => u.Customer)
                .WithOne(c => c.User)
                .HasForeignKey<Customer>(c => c.Id)
                .HasPrincipalKey<User>(u => u.Id);
            modelBuilder
                .Entity<User>()
                .HasOne(u => u.DeliverySite)
                .WithOne(c => c.User)
                .HasForeignKey<DeliverySite>(c => c.Id)
                .HasPrincipalKey<User>(u => u.Id);
            modelBuilder
                .Entity<User>()
                .HasMany(s => s.Addresses)
                .WithOne()
                .HasForeignKey(a => a.UserId);
            modelBuilder
                .Entity<User>()
                .HasMany(s => s.Contacts)
                .WithOne()
                .HasForeignKey(c => c.UserId);
            modelBuilder
                .Entity<User>()
                .HasMany(s => s.Payments)
                .WithOne()
                .HasForeignKey(c => c.UserId);
            modelBuilder
                .Entity<User>()
                .HasMany(s => s.ReceivedPayments)
                .WithOne()
                .HasForeignKey(c => c.TargetUserId);
            modelBuilder
                .Entity<User>()
                .HasMany(s => s.PaymentMethods)
                .WithOne()
                .HasForeignKey(c => c.UserId);
            modelBuilder
                .Entity<User>()
                .HasMany(s => s.Reviews)
                .WithOne()
                .HasForeignKey(s => s.UserId);
            modelBuilder
                .Entity<User>()
                .HasMany(s => s.ReceivedReviews)
                .WithOne()
                .HasForeignKey(s => s.TargetUserId);
            modelBuilder
                .Entity<User>()
                .Property(s => s.Salt)
                .IsRequired()
                .HasMaxLength(100);

            //Supplier entity
            modelBuilder
                .Entity<Supplier>()
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder
                .Entity<Supplier>()
                .Property(s => s.Description)
                .IsRequired()
                .HasMaxLength(400);
            modelBuilder
                .Entity<Supplier>()
                .HasMany(s => s.Products)
                .WithOne()
                .HasForeignKey(s => s.SupplierId);

            modelBuilder
                .Entity<Address>()
                .HasKey(a => new { a.UserId, a.AddressId });
            modelBuilder
                .Entity<Address>()
                .Property(a => a.CountryId)
                .IsUnicode(false);
            modelBuilder
                .Entity<Address>()
                .Property(a => a.StateId)
                .IsUnicode(false);
            modelBuilder
                .Entity<Address>()
                .HasOne(a => a.State)
                .WithMany()
                .HasForeignKey(a => a.StateId)
                .IsRequired(false);
            modelBuilder
                .Entity<Address>()
                .HasOne(a => a.Country)
                .WithMany()
                .HasForeignKey(a => a.CountryId)
                .IsRequired();
            modelBuilder
                .Entity<Address>(e =>
                {
                    e.Property(a => a.City)
                        .IsRequired()
                        .HasMaxLength(100);
                    e.Property(a => a.Street)
                        .IsRequired()
                        .HasMaxLength(400);
                    e.Property(a => a.Zip)
                        .IsUnicode(false)
                        .HasMaxLength(30);
                });

            modelBuilder
                .Entity<Contact>()
                .HasKey(c => new { c.UserId, c.ContactId });
            modelBuilder
                .Entity<Contact>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder
                .Entity<Contact>()
                .Property(c => c.Address)
                .IsRequired()
                .HasMaxLength(255);
            modelBuilder
                .Entity<Contact>()
                .HasMany(c => c.OrderSubscriptions)
                .WithOne()
                .HasForeignKey(s => new { s.UserId, s.ContactId })
                .HasPrincipalKey(c => new { c.UserId, c.ContactId });
            modelBuilder
                .Entity<Contact>()
                .HasMany(c => c.ReportSubscriptions)
                .WithOne()
                .HasForeignKey(s => new { s.UserId, s.ContactId })
                .HasPrincipalKey(c => new { c.UserId, c.ContactId });

            modelBuilder
                .Entity<OrderSubscription>()
                .HasKey(s => new { s.UserId, s.ContactId, s.CustomerId, s.OrderId });

            modelBuilder
                .Entity<ReportSubscription>()
                .HasKey(s => new { s.UserId, s.ContactId, s.Type });

            modelBuilder
                .Entity<Review>()
                .HasIndex(r => new { r.TargetUserId, r.Date });

            //modelBuilder
            //    .Entity<Review>()
            //    .HasIndex(r => new { r.UserId });

            modelBuilder
                .Entity<Review>()
                .Property(r => r.Feedback)
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
                .HasMany(p => p.ProductReviews)
                .WithOne()
                .HasForeignKey(r => new { r.SupplierId, r.ProductId });
            modelBuilder
                .Entity<ProductReview>()
                .HasKey(r => r.ReviewId);
            modelBuilder
                .Entity<ProductReview>()
                .HasOne(p => p.Review)
                .WithOne()
                .HasForeignKey<ProductReview>(r => r.ReviewId);
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
                .Entity<Customer>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder
                .Entity<Customer>()
                .HasMany(c => c.Orders)
                .WithOne(c => c.Customer)
                .HasForeignKey(o => o.CustomerId);

            modelBuilder
                .Entity<CartItem>()
                .HasKey(i => new { i.SessionId, i.ItemId });

            modelBuilder
                .Entity<CartItem>()
                .HasOne(c => c.Product)
                .WithMany()
                .HasForeignKey(i => new { i.SupplierId, i.ProductId })
                .HasPrincipalKey(c => new { c.SupplierId, c.ProductId });

            modelBuilder
                .Entity<Session>()
                .HasMany(s => s.CartItems)
                .WithOne()
                .HasForeignKey(c => c.SessionId)
                .HasPrincipalKey(s => s.Id);

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
                .Entity<DeliverySite>()
                .HasMany(d => d.Deliveries)
                .WithOne()
                .HasForeignKey(d => d.DeliverySiteId);

            modelBuilder
                .Entity<Delivery>()
                .HasKey(d => new { d.CustomerId, d.OrderId });

            modelBuilder
                .Entity<Delivery>()
                .HasIndex(d => new { d.DeliverySiteId });

            modelBuilder
                .Entity<Delivery>()
                .HasOne(d => d.Order)
                .WithOne()
                .HasForeignKey<Delivery>(d => new { d.CustomerId, d.OrderId });

            modelBuilder
                .Entity<Delivery>()
                .HasOne(d => d.CustomerAddress)
                .WithMany()
                .HasForeignKey(d => new { d.CustomerId, d.CustomerAddressId });

            modelBuilder
                .Entity<Delivery>()
                .HasOne(d => d.SupplierAddress)
                .WithMany()
                .HasForeignKey(d => new { d.SupplierId, d.SupplierAddressId });

            modelBuilder
                .Entity<PaymentMethod>()
                .HasKey(pm => new { pm.UserId, pm.PaymentMethodId });

            modelBuilder
                .Entity<PaymentMethod>()
                .HasMany(p => p.Payments)
                .WithOne()
                .HasForeignKey(p => new { p.UserId, p.PaymentMethodId });

            modelBuilder
                .Entity<CardPaymentMethod>()
                .HasKey(pm => new { pm.UserId, pm.PaymentMethodId });

            modelBuilder
                .Entity<CardPaymentMethod>()
                .HasOne(c => c.PaymentMethod)
                .WithOne()
                .HasForeignKey<CardPaymentMethod>(c => new { c.UserId, c.PaymentMethodId });

            modelBuilder
                .Entity<CardPaymentMethod>()
                .HasOne(c => c.BillingAddress)
                .WithMany()
                .HasForeignKey(c => new { c.UserId, c.BillingAddressId });

            modelBuilder
                .Entity<PayPalPaymentMethod>()
                .HasKey(pm => new { pm.UserId, pm.PaymentMethodId });

            modelBuilder
                .Entity<PayPalPaymentMethod>()
                .HasOne(c => c.PaymentMethod)
                .WithOne()
                .HasForeignKey<PayPalPaymentMethod>(c => new { c.UserId, c.PaymentMethodId });

            modelBuilder
                .Entity<CardPaymentMethod>(e =>
                {
                    e.Property(c => c.NameOnCard)
                        .IsRequired(true)
                        .HasMaxLength(100);
                    e.Property(c => c.CardNumber)
                        .IsRequired(true)
                        .HasMaxLength(16);
                });

            modelBuilder.Entity<Payment>()
                .HasKey(p => new { p.UserId, p.PaymentId });

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithOne()
                .HasForeignKey<Payment>(p => new { p.CustomerId, p.OrderId });

            modelBuilder.Entity<Payment>()
                .HasIndex(p => p.TargetUserId);

            modelBuilder.Entity<ProductImage>()
                .HasKey(i => new { i.SupplierId, i.ProductId, i.ImageId });

            modelBuilder.Entity<Supplier>()
                .Property(s => s.HasLogo)
                .HasDefaultValue(false);

            modelBuilder.Entity<Handshake>()
                .HasIndex(s => new { s.CustomerId, s.OrderId });

            modelBuilder.Entity<Handshake>()
                .HasMany(h => h.Comments)
                .WithOne()
                .HasForeignKey(c => c.HandshakeId);

            modelBuilder.Entity<Contact>()
                .HasMany(h => h.Sendings)
                .WithOne()
                .HasForeignKey(s => new { s.UserId, s.ContactId });

            modelBuilder
                .Entity<Supplier>()
                .HasMany(c => c.Orders)
                .WithOne(c => c.Supplier)
                .HasForeignKey(o => o.SupplierId);

            modelBuilder
                .Entity<OrderPaymentSensitiveInfo>()
                .HasOne(c => c.Order)
                .WithOne()
                .HasForeignKey<OrderPaymentSensitiveInfo>(o => new { o.CustomerId, o.OrderId });

            modelBuilder
                .Entity<OrderPaymentSensitiveInfo>()
                .HasOne(c => c.PaymentMethod)
                .WithMany()
                .HasForeignKey(o => new { o.CustomerId, o.PaymentMethodId });

            modelBuilder
                .Entity<Order>()
                .HasMany(c => c.Subscriptions)
                .WithOne()
                .HasForeignKey(s => new { s.CustomerId, s.OrderId });

            modelBuilder
                .Entity<HandshakeComment>()
                .HasKey(hc => new { hc.HandshakeId, hc.ProductId });

            modelBuilder
                .Entity<NotificationLog>()
                .HasKey(n => new { n.UserId, n.ContactId, n.MessageId });

            modelBuilder
                .Entity<OrderPaymentSensitiveInfo>()
                .HasKey(o => new { o.CustomerId, o.OrderId });

            //datetime2 precisions
            modelBuilder
                .Entity<CartItem>()
                .Property(i => i.DatePlaced)
                .HasColumnType("datetime2(0)");

            modelBuilder
                .Entity<Product>()
                .Property(i => i.LastUpdated)
                .IsConcurrencyToken();

            modelBuilder
                .Entity<Handshake>()
                .Property(i => i.TimeStamp)
                .HasColumnType("datetime2(0)");

            modelBuilder
                .Entity<NotificationLog>()
                .Property(i => i.Date)
                .HasColumnType("datetime2(0)");

            modelBuilder
                .Entity<Order>()
                .Property(i => i.DatePlaced)
                .HasColumnType("datetime2(0)");

            modelBuilder
                .Entity<Order>()
                .Property(i => i.LastUpdated)
                .IsConcurrencyToken();

            modelBuilder
                .Entity<Order>()
                .Property(i => i.PlannedDeliveryDate)
                .HasColumnType("datetime2(0)");

            modelBuilder
                .Entity<Session>()
                .Property(i => i.FirstVisit)
                .HasColumnType("datetime2(0)");

            modelBuilder
                .Entity<Session>()
                .Property(i => i.LastVisit)
                .HasColumnType("datetime2(0)");

            modelBuilder
                .Entity<Review>()
                .Property(i => i.Date)
                .HasColumnType("datetime2(0)");

            modelBuilder
                .Entity<Delivery>()
                .Property(i => i.LastUpdated)
                .IsConcurrencyToken();

            modelBuilder
                .Entity<Handshake>()
                .HasOne(h => h.Order)
                .WithMany()
                .HasForeignKey(h => new { h.CustomerId, h.OrderId });

            modelBuilder
                .Entity<ProductCategory>()
                .Property(p => p.Id)
                .ValueGeneratedNever();

            modelBuilder
                .Entity<HandshakeComment>()
                .Property(c => c.Comment)
                .IsRequired()
                .HasMaxLength(400);

            modelBuilder
                .Entity<PaymentMethod>()
                .Property(m => m.UIDesc)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder
                .Entity<NotificationLog>()
                .Property(l => l.Subject)
                .HasMaxLength(255);
            modelBuilder
                .Entity<NotificationLog>()
                .Property(l => l.Text)
                .IsRequired();
            modelBuilder
                .Entity<PayPalPaymentMethod>()
                .Property(p => p.PayPalAddress)
                .IsRequired()
                .HasMaxLength(255);
            modelBuilder
                .Entity<OrderPaymentSensitiveInfo>()
                .Property(i => i.CVV)
                .HasMaxLength(100).IsRequired();
            modelBuilder
                .Entity<OrderPaymentSensitiveInfo>()
                .Property(i => i.ExpirationDate)
                .HasMaxLength(100).IsRequired();
            modelBuilder
                .Entity<ProductImage>()
                .Property(i => i.LogicalPath)
                .HasMaxLength(100).IsRequired();
            modelBuilder
                .Entity<ProductImage>()
                .Property(i => i.Path)
                .HasMaxLength(255).IsRequired();
            modelBuilder
                .Entity<ProductImage>()
                .Property(i => i.Mime)
                .HasMaxLength(40).IsRequired();

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
