﻿// <auto-generated />
using BakeryHub.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace BakeryHub.Domain.Migrations
{
    [DbContext(typeof(BakeryHubContext))]
    [Migration("20171203004449_NotificationLogPartitioning")]
    partial class NotificationLogPartitioning
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BakeryHub.Domain.Address", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("AddressId");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("CountryId")
                        .IsRequired()
                        .IsUnicode(false);

                    b.Property<bool>("IsBilling");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("StateId")
                        .IsUnicode(false);

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(400);

                    b.Property<string>("Zip")
                        .HasMaxLength(30)
                        .IsUnicode(false);

                    b.HasKey("UserId", "AddressId");

                    b.HasIndex("CountryId");

                    b.HasIndex("StateId");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("BakeryHub.Domain.CardPaymentMethod", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("PaymentMethodId");

                    b.Property<int>("BillingAddressId");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasMaxLength(16);

                    b.Property<string>("NameOnCard")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("UserId", "PaymentMethodId");

                    b.HasIndex("UserId", "BillingAddressId");

                    b.ToTable("CardPaymentMethods");
                });

            modelBuilder.Entity("BakeryHub.Domain.CartItem", b =>
                {
                    b.Property<Guid>("SessionId");

                    b.Property<int>("ItemId");

                    b.Property<DateTime>("DatePlaced")
                        .HasColumnType("datetime2(0)");

                    b.Property<int>("ProductCount");

                    b.Property<int>("ProductId");

                    b.Property<int>("SupplierId");

                    b.HasKey("SessionId", "ItemId");

                    b.HasIndex("SupplierId", "ProductId");

                    b.ToTable("CartItems");
                });

            modelBuilder.Entity("BakeryHub.Domain.Contact", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("ContactId");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<bool>("IsConfirmed");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsPrivate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("Type");

                    b.HasKey("UserId", "ContactId");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("BakeryHub.Domain.Country", b =>
                {
                    b.Property<string>("Code")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(2)
                        .IsUnicode(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.HasKey("Code");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("BakeryHub.Domain.CountryState", b =>
                {
                    b.Property<string>("Code")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(2)
                        .IsUnicode(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.HasKey("Code");

                    b.ToTable("States");
                });

            modelBuilder.Entity("BakeryHub.Domain.Customer", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("BakeryHub.Domain.Delivery", b =>
                {
                    b.Property<int>("CustomerId");

                    b.Property<int>("OrderId");

                    b.Property<int>("CustomerAddressId");

                    b.Property<int>("DeliverySiteId");

                    b.Property<DateTime>("LastUpdated")
                        .IsConcurrencyToken();

                    b.Property<decimal?>("Price");

                    b.Property<int>("Status");

                    b.Property<int>("SupplierAddressId");

                    b.Property<int>("SupplierId");

                    b.HasKey("CustomerId", "OrderId");

                    b.HasIndex("DeliverySiteId");

                    b.HasIndex("CustomerId", "CustomerAddressId");

                    b.HasIndex("SupplierId", "SupplierAddressId");

                    b.ToTable("Delivery");
                });

            modelBuilder.Entity("BakeryHub.Domain.DeliverySite", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(400);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<bool>("isCompany");

                    b.HasKey("Id");

                    b.ToTable("DeliverySites");
                });

            modelBuilder.Entity("BakeryHub.Domain.Handshake", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomerId");

                    b.Property<int>("OrderId");

                    b.Property<int>("SeqNum");

                    b.Property<int>("SupplierId");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2(0)");

                    b.Property<int>("Turn");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId", "OrderId");

                    b.ToTable("Handshake");
                });

            modelBuilder.Entity("BakeryHub.Domain.HandshakeComment", b =>
                {
                    b.Property<Guid>("HandshakeId");

                    b.Property<int>("ProductId");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(400);

                    b.Property<bool>("IsCanceled");

                    b.Property<decimal?>("NewPrice");

                    b.HasKey("HandshakeId", "ProductId");

                    b.ToTable("HandshakeComments");
                });

            modelBuilder.Entity("BakeryHub.Domain.NotificationLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ContactId");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2(0)");

                    b.Property<string>("ErrorMessage");

                    b.Property<int>("Status");

                    b.Property<string>("Subject")
                        .HasMaxLength(255);

                    b.Property<string>("Text")
                        .IsRequired();

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId", "ContactId");

                    b.ToTable("NotificationLog");
                });

            modelBuilder.Entity("BakeryHub.Domain.Order", b =>
                {
                    b.Property<int>("CustomerId");

                    b.Property<int>("OrderId");

                    b.Property<DateTime>("DatePlaced")
                        .HasColumnType("datetime2(0)");

                    b.Property<DateTime>("LastUpdated")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("PlannedDeliveryDate")
                        .HasColumnType("datetime2(0)");

                    b.Property<decimal>("Price");

                    b.Property<int>("Status");

                    b.Property<int>("SupplierId");

                    b.HasKey("CustomerId", "OrderId");

                    b.HasIndex("SupplierId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("BakeryHub.Domain.OrderItem", b =>
                {
                    b.Property<int>("CustomerId");

                    b.Property<int>("OrderId");

                    b.Property<int>("ProductId");

                    b.Property<bool>("IsCanceled");

                    b.Property<int>("ProductCount");

                    b.Property<decimal>("TotalPrice");

                    b.HasKey("CustomerId", "OrderId", "ProductId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("BakeryHub.Domain.OrderPaymentSensitiveInfo", b =>
                {
                    b.Property<int>("CustomerId");

                    b.Property<int>("OrderId");

                    b.Property<string>("CVV")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("ExpirationDate")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("PaymentMethodId");

                    b.HasKey("CustomerId", "OrderId");

                    b.HasIndex("CustomerId", "PaymentMethodId");

                    b.ToTable("OrderPaymentSensitiveInfo");
                });

            modelBuilder.Entity("BakeryHub.Domain.OrderSubscription", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("ContactId");

                    b.Property<int>("CustomerId");

                    b.Property<int>("OrderId");

                    b.HasKey("UserId", "ContactId", "CustomerId", "OrderId");

                    b.HasIndex("CustomerId", "OrderId");

                    b.ToTable("OrderSubscription");
                });

            modelBuilder.Entity("BakeryHub.Domain.Payment", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("PaymentId");

                    b.Property<decimal>("Amount");

                    b.Property<int>("CustomerId");

                    b.Property<int>("OrderId");

                    b.Property<int>("PaymentMethodId");

                    b.Property<int>("Status");

                    b.Property<int>("Target");

                    b.Property<int>("TargetUserId");

                    b.HasKey("UserId", "PaymentId");

                    b.HasIndex("TargetUserId");

                    b.HasIndex("CustomerId", "OrderId")
                        .IsUnique();

                    b.HasIndex("UserId", "PaymentMethodId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("BakeryHub.Domain.PaymentMethod", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("PaymentMethodId");

                    b.Property<bool>("IsDeleted");

                    b.Property<int>("Type");

                    b.Property<string>("UIDesc")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("UserId", "PaymentMethodId");

                    b.ToTable("PaymentMethods");
                });

            modelBuilder.Entity("BakeryHub.Domain.PayPalPaymentMethod", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("PaymentMethodId");

                    b.Property<string>("PayPalAddress")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("UserId", "PaymentMethodId");

                    b.ToTable("PayPalPaymentMethods");
                });

            modelBuilder.Entity("BakeryHub.Domain.Product", b =>
                {
                    b.Property<int>("SupplierId");

                    b.Property<int>("ProductId");

                    b.Property<int>("AvailableNow");

                    b.Property<int>("CategoryId");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(400);

                    b.Property<DateTime>("LastUpdated")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<decimal>("Price");

                    b.HasKey("SupplierId", "ProductId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("BakeryHub.Domain.ProductCategory", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(400);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("ProductCategories");
                });

            modelBuilder.Entity("BakeryHub.Domain.ProductImage", b =>
                {
                    b.Property<int>("SupplierId");

                    b.Property<int>("ProductId");

                    b.Property<int>("ImageId");

                    b.Property<string>("LogicalPath")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Mime")
                        .IsRequired()
                        .HasMaxLength(40);

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("SupplierId", "ProductId", "ImageId");

                    b.ToTable("ProductImages");
                });

            modelBuilder.Entity("BakeryHub.Domain.ProductReview", b =>
                {
                    b.Property<Guid>("ReviewId");

                    b.Property<int>("ProductId");

                    b.Property<int>("SupplierId");

                    b.HasKey("ReviewId");

                    b.HasIndex("SupplierId", "ProductId");

                    b.ToTable("ProductReviews");
                });

            modelBuilder.Entity("BakeryHub.Domain.ReportSubscription", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("ContactId");

                    b.Property<int>("Type");

                    b.HasKey("UserId", "ContactId", "Type");

                    b.ToTable("ReportSubscription");
                });

            modelBuilder.Entity("BakeryHub.Domain.Review", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2(0)");

                    b.Property<string>("Feedback")
                        .IsRequired()
                        .HasMaxLength(400);

                    b.Property<bool>("IsAboutProduct");

                    b.Property<int>("Rating");

                    b.Property<int>("TargetUserId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("TargetUserId", "Date");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("BakeryHub.Domain.Session", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("FirstVisit")
                        .HasColumnType("datetime2(0)");

                    b.Property<string>("IP")
                        .IsRequired()
                        .HasMaxLength(39);

                    b.Property<DateTime>("LastVisit")
                        .HasColumnType("datetime2(0)");

                    b.Property<string>("UserAgent")
                        .IsRequired()
                        .HasMaxLength(400);

                    b.HasKey("Id");

                    b.ToTable("Session");
                });

            modelBuilder.Entity("BakeryHub.Domain.Supplier", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(400);

                    b.Property<bool>("HasLogo")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<bool>("IsApproved");

                    b.Property<bool>("IsCompany");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("BakeryHub.Domain.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<int>("PasswordEncryptionAlgorithm");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<Guid>("SessionId");

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.HasIndex("SessionId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BakeryHub.Domain.Address", b =>
                {
                    b.HasOne("BakeryHub.Domain.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BakeryHub.Domain.CountryState", "State")
                        .WithMany()
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BakeryHub.Domain.User")
                        .WithMany("Addresses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BakeryHub.Domain.CardPaymentMethod", b =>
                {
                    b.HasOne("BakeryHub.Domain.Address", "BillingAddress")
                        .WithMany()
                        .HasForeignKey("UserId", "BillingAddressId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BakeryHub.Domain.PaymentMethod", "PaymentMethod")
                        .WithOne()
                        .HasForeignKey("BakeryHub.Domain.CardPaymentMethod", "UserId", "PaymentMethodId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BakeryHub.Domain.CartItem", b =>
                {
                    b.HasOne("BakeryHub.Domain.Session")
                        .WithMany("CartItems")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BakeryHub.Domain.Product", "Product")
                        .WithMany()
                        .HasForeignKey("SupplierId", "ProductId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BakeryHub.Domain.Contact", b =>
                {
                    b.HasOne("BakeryHub.Domain.User")
                        .WithMany("Contacts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BakeryHub.Domain.Customer", b =>
                {
                    b.HasOne("BakeryHub.Domain.User", "User")
                        .WithOne("Customer")
                        .HasForeignKey("BakeryHub.Domain.Customer", "Id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BakeryHub.Domain.Delivery", b =>
                {
                    b.HasOne("BakeryHub.Domain.DeliverySite")
                        .WithMany("Deliveries")
                        .HasForeignKey("DeliverySiteId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BakeryHub.Domain.Address", "CustomerAddress")
                        .WithMany()
                        .HasForeignKey("CustomerId", "CustomerAddressId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BakeryHub.Domain.Order", "Order")
                        .WithOne()
                        .HasForeignKey("BakeryHub.Domain.Delivery", "CustomerId", "OrderId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BakeryHub.Domain.Address", "SupplierAddress")
                        .WithMany()
                        .HasForeignKey("SupplierId", "SupplierAddressId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BakeryHub.Domain.DeliverySite", b =>
                {
                    b.HasOne("BakeryHub.Domain.User", "User")
                        .WithOne("DeliverySite")
                        .HasForeignKey("BakeryHub.Domain.DeliverySite", "Id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BakeryHub.Domain.Handshake", b =>
                {
                    b.HasOne("BakeryHub.Domain.Order", "Order")
                        .WithMany()
                        .HasForeignKey("CustomerId", "OrderId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BakeryHub.Domain.HandshakeComment", b =>
                {
                    b.HasOne("BakeryHub.Domain.Handshake")
                        .WithMany("Comments")
                        .HasForeignKey("HandshakeId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BakeryHub.Domain.NotificationLog", b =>
                {
                    b.HasOne("BakeryHub.Domain.Contact")
                        .WithMany("Sendings")
                        .HasForeignKey("UserId", "ContactId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BakeryHub.Domain.Order", b =>
                {
                    b.HasOne("BakeryHub.Domain.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BakeryHub.Domain.Supplier", "Supplier")
                        .WithMany("Orders")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BakeryHub.Domain.OrderItem", b =>
                {
                    b.HasOne("BakeryHub.Domain.Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("CustomerId", "OrderId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BakeryHub.Domain.OrderPaymentSensitiveInfo", b =>
                {
                    b.HasOne("BakeryHub.Domain.Order", "Order")
                        .WithOne()
                        .HasForeignKey("BakeryHub.Domain.OrderPaymentSensitiveInfo", "CustomerId", "OrderId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BakeryHub.Domain.PaymentMethod", "PaymentMethod")
                        .WithMany()
                        .HasForeignKey("CustomerId", "PaymentMethodId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BakeryHub.Domain.OrderSubscription", b =>
                {
                    b.HasOne("BakeryHub.Domain.Order")
                        .WithMany("Subscriptions")
                        .HasForeignKey("CustomerId", "OrderId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BakeryHub.Domain.Contact")
                        .WithMany("OrderSubscriptions")
                        .HasForeignKey("UserId", "ContactId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BakeryHub.Domain.Payment", b =>
                {
                    b.HasOne("BakeryHub.Domain.User")
                        .WithMany("ReceivedPayments")
                        .HasForeignKey("TargetUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BakeryHub.Domain.User")
                        .WithMany("Payments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BakeryHub.Domain.Order", "Order")
                        .WithOne()
                        .HasForeignKey("BakeryHub.Domain.Payment", "CustomerId", "OrderId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BakeryHub.Domain.PaymentMethod")
                        .WithMany("Payments")
                        .HasForeignKey("UserId", "PaymentMethodId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BakeryHub.Domain.PaymentMethod", b =>
                {
                    b.HasOne("BakeryHub.Domain.User")
                        .WithMany("PaymentMethods")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BakeryHub.Domain.PayPalPaymentMethod", b =>
                {
                    b.HasOne("BakeryHub.Domain.PaymentMethod", "PaymentMethod")
                        .WithOne()
                        .HasForeignKey("BakeryHub.Domain.PayPalPaymentMethod", "UserId", "PaymentMethodId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BakeryHub.Domain.Product", b =>
                {
                    b.HasOne("BakeryHub.Domain.ProductCategory", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BakeryHub.Domain.Supplier")
                        .WithMany("Products")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BakeryHub.Domain.ProductImage", b =>
                {
                    b.HasOne("BakeryHub.Domain.Product")
                        .WithMany("Images")
                        .HasForeignKey("SupplierId", "ProductId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BakeryHub.Domain.ProductReview", b =>
                {
                    b.HasOne("BakeryHub.Domain.Review", "Review")
                        .WithOne()
                        .HasForeignKey("BakeryHub.Domain.ProductReview", "ReviewId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BakeryHub.Domain.Product")
                        .WithMany("ProductReviews")
                        .HasForeignKey("SupplierId", "ProductId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BakeryHub.Domain.ReportSubscription", b =>
                {
                    b.HasOne("BakeryHub.Domain.Contact")
                        .WithMany("ReportSubscriptions")
                        .HasForeignKey("UserId", "ContactId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BakeryHub.Domain.Review", b =>
                {
                    b.HasOne("BakeryHub.Domain.User")
                        .WithMany("ReceivedReviews")
                        .HasForeignKey("TargetUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BakeryHub.Domain.User")
                        .WithMany("Reviews")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BakeryHub.Domain.Supplier", b =>
                {
                    b.HasOne("BakeryHub.Domain.User", "User")
                        .WithOne("Supplier")
                        .HasForeignKey("BakeryHub.Domain.Supplier", "Id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BakeryHub.Domain.User", b =>
                {
                    b.HasOne("BakeryHub.Domain.Session", "Session")
                        .WithMany()
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
