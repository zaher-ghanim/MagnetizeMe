﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RepositoryAPI;

#nullable disable

namespace RepositoryAPI.Migrations
{
    [DbContext(typeof(EcommerceDbContext))]
    [Migration("20250113080937_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RepositoryAPI.Models.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("item_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ItemId"));

                    b.Property<string>("ItemDesc")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("item_desc");

                    b.HasKey("ItemId");

                    b.ToTable("Item", (string)null);
                });

            modelBuilder.Entity("RepositoryAPI.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("order_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("address");

                    b.Property<string>("Date")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nchar(10)")
                        .HasColumnName("date")
                        .IsFixedLength();

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("phone");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18, 0)")
                        .HasColumnName("price");

                    b.Property<int>("SizeId")
                        .HasColumnType("int")
                        .HasColumnName("size_id");

                    b.Property<bool>("status")
                        .HasColumnType("bit")
                        .HasColumnName("status");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("OrderId");

                    b.HasIndex("SizeId");

                    b.HasIndex("UserId");

                    b.ToTable("Order", (string)null);
                });

            modelBuilder.Entity("RepositoryAPI.Models.Size", b =>
                {
                    b.Property<int>("SizeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("size_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SizeId"));

                    b.Property<string>("Dimension")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("dimension");

                    b.Property<int>("ItemId")
                        .HasColumnType("int")
                        .HasColumnName("item_id");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18, 0)")
                        .HasColumnName("price");

                    b.Property<int>("Quantity")
                        .HasColumnType("int")
                        .HasColumnName("quantity");

                    b.Property<string>("SizeDesc")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("size_desc");

                    b.Property<decimal>("StepQuantity")
                        .HasColumnType("decimal(18, 0)")
                        .HasColumnName("step_quantity");

                    b.HasKey("SizeId");

                    b.HasIndex("ItemId");

                    b.ToTable("Size", (string)null);
                });

            modelBuilder.Entity("RepositoryAPI.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("address");

                    b.Property<DateOnly>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("first_name");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit")
                        .HasColumnName("is_admin");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("last_name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("password");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("phone");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("user_name");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RepositoryAPI.Models.Order", b =>
                {
                    b.HasOne("RepositoryAPI.Models.Size", "Size")
                        .WithMany("Orders")
                        .HasForeignKey("SizeId")
                        .IsRequired()
                        .HasConstraintName("FK_Order_Size");

                    b.HasOne("RepositoryAPI.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_Order_Users");

                    b.Navigation("Size");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RepositoryAPI.Models.Size", b =>
                {
                    b.HasOne("RepositoryAPI.Models.Item", "Item")
                        .WithMany("Sizes")
                        .HasForeignKey("ItemId")
                        .IsRequired()
                        .HasConstraintName("FK_Size_Item");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("RepositoryAPI.Models.Item", b =>
                {
                    b.Navigation("Sizes");
                });

            modelBuilder.Entity("RepositoryAPI.Models.Size", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("RepositoryAPI.Models.User", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
