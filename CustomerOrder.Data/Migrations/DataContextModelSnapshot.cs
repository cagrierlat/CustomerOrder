﻿// <auto-generated />
using System;
using CustomerOrder.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CustomerOrder.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("CustomerOrder.Data.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("Id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("integer")
                        .HasColumnName("CreatedBy");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("CreatedDate");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("IsDeleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("integer")
                        .HasColumnName("UpdatedBy");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("UpdatedDate");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("CustomerOrder.Data.Models.CustomerOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("Id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("CreatedBy")
                        .HasColumnType("integer")
                        .HasColumnName("CreatedBy");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("CreatedDate");

                    b.Property<int>("CustomerId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("IsDeleted");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("integer")
                        .HasColumnName("UpdatedBy");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("CustomerOrders");
                });

            modelBuilder.Entity("CustomerOrder.Data.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("Id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Barcode")
                        .HasColumnType("text");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("integer")
                        .HasColumnName("CreatedBy");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("CreatedDate");

                    b.Property<int>("CustomerOrderId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("IsDeleted");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("integer")
                        .HasColumnName("UpdatedBy");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("CustomerOrderId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("CustomerOrder.Data.Models.CustomerOrder", b =>
                {
                    b.HasOne("CustomerOrder.Data.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("CustomerOrder.Data.Models.Product", b =>
                {
                    b.HasOne("CustomerOrder.Data.Models.CustomerOrder", "CustomerOrder")
                        .WithMany("Products")
                        .HasForeignKey("CustomerOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CustomerOrder");
                });

            modelBuilder.Entity("CustomerOrder.Data.Models.CustomerOrder", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
