﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ReactApp1.Server.Data;

#nullable disable

namespace ReactApp1.Server.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FullOrderItem", b =>
                {
                    b.Property<int>("FullOrdersFullOrderId")
                        .HasColumnType("integer");

                    b.Property<int>("ItemsItemId")
                        .HasColumnType("integer");

                    b.HasKey("FullOrdersFullOrderId", "ItemsItemId");

                    b.HasIndex("ItemsItemId");

                    b.ToTable("FullOrderItem");
                });

            modelBuilder.Entity("ReactApp1.Server.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("EmployeeId");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("EmployeeId"));

                    b.Property<int>("AddressId")
                        .HasColumnType("integer")
                        .HasColumnName("AddressId");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("BirthDate");

                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("Email");

                    b.Property<int>("EstablishmentId")
                        .HasColumnType("integer")
                        .HasColumnName("EstablishmentId");

                    b.Property<string>("FirstName")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("FirstName");

                    b.Property<string>("LastName")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("LastName");

                    b.Property<string>("PasswordHash")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("PasswordHash");

                    b.Property<string>("PersonalCode")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("PersonalCode");

                    b.Property<string>("Phone")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("Phone");

                    b.Property<DateTime>("ReceiveTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("ReceiveTime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<int>("Title")
                        .HasColumnType("integer")
                        .HasColumnName("Title");

                    b.HasKey("EmployeeId");

                    b.HasIndex("EstablishmentId");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("ReactApp1.Server.Models.EmployeeAddress", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("AddressId");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AddressId"));

                    b.Property<string>("City")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("City");

                    b.Property<string>("Country")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("Country");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("integer")
                        .HasColumnName("EmployeeId");

                    b.Property<string>("HouseNumber")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("HouseNumber");

                    b.Property<DateTime>("ReceiveTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("ReceiveTime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Street")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("Street");

                    b.Property<string>("StreetNumber")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("StreetNumber");

                    b.HasKey("AddressId");

                    b.HasIndex("EmployeeId")
                        .IsUnique();

                    b.ToTable("EmployeeAddress");
                });

            modelBuilder.Entity("ReactApp1.Server.Models.Establishment", b =>
                {
                    b.Property<int>("EstablishmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("EstablishmentId");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("EstablishmentId"));

                    b.Property<int>("EstablishmentAddressId")
                        .HasColumnType("integer")
                        .HasColumnName("EstablishmentAddressId");

                    b.Property<DateTime>("ReceiveTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("ReceiveTime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("Type");

                    b.HasKey("EstablishmentId");

                    b.ToTable("Establishment");
                });

            modelBuilder.Entity("ReactApp1.Server.Models.EstablishmentAddress", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("AddressId");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AddressId"));

                    b.Property<string>("City")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("City");

                    b.Property<string>("Country")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("Country");

                    b.Property<int>("EstablishmentId")
                        .HasColumnType("integer")
                        .HasColumnName("EstablishmentId");

                    b.Property<DateTime>("ReceiveTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("ReceiveTime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Street")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("Street");

                    b.Property<string>("StreetNumber")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("StreetNumber");

                    b.HasKey("AddressId");

                    b.HasIndex("EstablishmentId")
                        .IsUnique();

                    b.ToTable("EstablishmentAddress");
                });

            modelBuilder.Entity("ReactApp1.Server.Models.FullOrder", b =>
                {
                    b.Property<int>("FullOrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("FullOrderId"));

                    b.Property<int>("Count")
                        .HasColumnType("integer")
                        .HasColumnName("Count");

                    b.Property<int>("ItemId")
                        .HasColumnType("integer")
                        .HasColumnName("ItemId");

                    b.Property<int>("OrderId")
                        .HasColumnType("integer")
                        .HasColumnName("OrderId");

                    b.HasKey("FullOrderId");

                    b.HasIndex("OrderId");

                    b.ToTable("FullOrder");
                });

            modelBuilder.Entity("ReactApp1.Server.Models.GiftCard", b =>
                {
                    b.Property<int>("GiftCardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("GiftCardId");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("GiftCardId"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric")
                        .HasColumnName("Amount");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("Code");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("ExpirationDate");

                    b.Property<int>("PaymentId")
                        .HasColumnType("integer")
                        .HasColumnName("PaymentId");

                    b.Property<DateTime>("ReceiveTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("ReceiveTime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.HasKey("GiftCardId");

                    b.HasIndex("PaymentId");

                    b.ToTable("GiftCard");
                });

            modelBuilder.Entity("ReactApp1.Server.Models.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("ItemId");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ItemId"));

                    b.Property<bool>("AlcoholicBeverage")
                        .HasColumnType("boolean")
                        .HasColumnName("AlcoholicBeverage");

                    b.Property<decimal?>("Cost")
                        .HasColumnType("numeric")
                        .HasColumnName("Cost");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("Name");

                    b.Property<DateTime>("ReceiveTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("ReceiveTime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<decimal?>("Tax")
                        .HasColumnType("numeric")
                        .HasColumnName("Tax");

                    b.HasKey("ItemId");

                    b.ToTable("Item");
                });

            modelBuilder.Entity("ReactApp1.Server.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("OrderId");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("OrderId"));

                    b.Property<int>("CreatedByEmployeeId")
                        .HasColumnType("integer")
                        .HasColumnName("CreatedByEmployeeId");

                    b.Property<decimal?>("DiscountFixed")
                        .HasColumnType("numeric")
                        .HasColumnName("DiscountFixed");

                    b.Property<int?>("DiscountPercentage")
                        .HasColumnType("integer")
                        .HasColumnName("DiscountPercentage");

                    b.Property<int?>("PaymentId")
                        .HasColumnType("integer")
                        .HasColumnName("PaymentId");

                    b.Property<DateTime>("ReceiveTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("ReceiveTime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<bool>("Refunded")
                        .HasColumnType("boolean")
                        .HasColumnName("Refunded");

                    b.Property<int?>("ReservationId")
                        .HasColumnType("integer")
                        .HasColumnName("ReservationId");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("Status");

                    b.HasKey("OrderId");

                    b.HasIndex("CreatedByEmployeeId");

                    b.HasIndex("ReservationId")
                        .IsUnique();

                    b.ToTable("Order");
                });

            modelBuilder.Entity("ReactApp1.Server.Models.Payment", b =>
                {
                    b.Property<int>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("PaymentId");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PaymentId"));

                    b.Property<int>("GiftCardId")
                        .HasColumnType("integer")
                        .HasColumnName("GiftCardId");

                    b.Property<int>("OrderId")
                        .HasColumnType("integer")
                        .HasColumnName("OrderId");

                    b.Property<DateTime>("ReceiveTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("ReceiveTime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<decimal?>("TipFixed")
                        .HasColumnType("numeric")
                        .HasColumnName("TipFixed");

                    b.Property<int?>("TipPercentage")
                        .HasColumnType("integer")
                        .HasColumnName("TipPercentage");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("Type");

                    b.HasKey("PaymentId");

                    b.HasIndex("OrderId");

                    b.ToTable("Table");
                });

            modelBuilder.Entity("ReactApp1.Server.Models.Reservation", b =>
                {
                    b.Property<int>("ReservationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("ReservationId");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ReservationId"));

                    b.Property<int?>("CustomerCount")
                        .HasColumnType("integer")
                        .HasColumnName("CustomerCount");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("EndTime");

                    b.Property<DateTime>("ReceiveTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("ReceiveTime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("ReservedSpot")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("ReservedSpot");

                    b.Property<DateTime?>("StartTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("StartTime");

                    b.HasKey("ReservationId");

                    b.ToTable("Reservation");
                });

            modelBuilder.Entity("ReactApp1.Server.Models.Storage", b =>
                {
                    b.Property<int>("StorageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("StorageId");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("StorageId"));

                    b.Property<int>("Count")
                        .HasColumnType("integer")
                        .HasColumnName("Count");

                    b.Property<int>("EstablishmentId")
                        .HasColumnType("integer")
                        .HasColumnName("EstablishmentId");

                    b.Property<int>("ItemId")
                        .HasColumnType("integer")
                        .HasColumnName("ItemId");

                    b.HasKey("StorageId");

                    b.HasIndex("EstablishmentId");

                    b.HasIndex("ItemId")
                        .IsUnique();

                    b.ToTable("Storage");
                });

            modelBuilder.Entity("ReactApp1.Server.Models.TestModel", b =>
                {
                    b.Property<Guid>("GuidId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("GuidId")
                        .HasDefaultValueSql("gen_random_uuid()")
                        .HasComment("Guid identification");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CreatedAt")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP")
                        .HasComment("Timestamp when the record was created");

                    b.Property<int>("IntId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("IntId")
                        .HasComment("Integer identification");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IntId"));

                    b.HasKey("GuidId");

                    b.ToTable("Test", t =>
                        {
                            t.HasComment("Table for testing purposes");
                        });
                });

            modelBuilder.Entity("FullOrderItem", b =>
                {
                    b.HasOne("ReactApp1.Server.Models.FullOrder", null)
                        .WithMany()
                        .HasForeignKey("FullOrdersFullOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ReactApp1.Server.Models.Item", null)
                        .WithMany()
                        .HasForeignKey("ItemsItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ReactApp1.Server.Models.Employee", b =>
                {
                    b.HasOne("ReactApp1.Server.Models.Establishment", "Establishment")
                        .WithMany("Employees")
                        .HasForeignKey("EstablishmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Establishment");
                });

            modelBuilder.Entity("ReactApp1.Server.Models.EmployeeAddress", b =>
                {
                    b.HasOne("ReactApp1.Server.Models.Employee", "Employee")
                        .WithOne("EmployeeAddress")
                        .HasForeignKey("ReactApp1.Server.Models.EmployeeAddress", "EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("ReactApp1.Server.Models.EstablishmentAddress", b =>
                {
                    b.HasOne("ReactApp1.Server.Models.Establishment", "Establishment")
                        .WithOne("EstablishmentAddress")
                        .HasForeignKey("ReactApp1.Server.Models.EstablishmentAddress", "EstablishmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Establishment");
                });

            modelBuilder.Entity("ReactApp1.Server.Models.FullOrder", b =>
                {
                    b.HasOne("ReactApp1.Server.Models.Order", "Order")
                        .WithMany("FullOrders")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("ReactApp1.Server.Models.GiftCard", b =>
                {
                    b.HasOne("ReactApp1.Server.Models.Payment", "Payment")
                        .WithMany("GiftCards")
                        .HasForeignKey("PaymentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Payment");
                });

            modelBuilder.Entity("ReactApp1.Server.Models.Order", b =>
                {
                    b.HasOne("ReactApp1.Server.Models.Employee", "CreatedByEmployee")
                        .WithMany("Orders")
                        .HasForeignKey("CreatedByEmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ReactApp1.Server.Models.Reservation", "Reservation")
                        .WithOne("Order")
                        .HasForeignKey("ReactApp1.Server.Models.Order", "ReservationId");

                    b.Navigation("CreatedByEmployee");

                    b.Navigation("Reservation");
                });

            modelBuilder.Entity("ReactApp1.Server.Models.Payment", b =>
                {
                    b.HasOne("ReactApp1.Server.Models.Order", "Order")
                        .WithMany("Payments")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("ReactApp1.Server.Models.Storage", b =>
                {
                    b.HasOne("ReactApp1.Server.Models.Establishment", "Establishment")
                        .WithMany("Storages")
                        .HasForeignKey("EstablishmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ReactApp1.Server.Models.Item", "Item")
                        .WithOne("Storage")
                        .HasForeignKey("ReactApp1.Server.Models.Storage", "ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Establishment");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("ReactApp1.Server.Models.Employee", b =>
                {
                    b.Navigation("EmployeeAddress")
                        .IsRequired();

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("ReactApp1.Server.Models.Establishment", b =>
                {
                    b.Navigation("Employees");

                    b.Navigation("EstablishmentAddress");

                    b.Navigation("Storages");
                });

            modelBuilder.Entity("ReactApp1.Server.Models.Item", b =>
                {
                    b.Navigation("Storage");
                });

            modelBuilder.Entity("ReactApp1.Server.Models.Order", b =>
                {
                    b.Navigation("FullOrders");

                    b.Navigation("Payments");
                });

            modelBuilder.Entity("ReactApp1.Server.Models.Payment", b =>
                {
                    b.Navigation("GiftCards");
                });

            modelBuilder.Entity("ReactApp1.Server.Models.Reservation", b =>
                {
                    b.Navigation("Order")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
