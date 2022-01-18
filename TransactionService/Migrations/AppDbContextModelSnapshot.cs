﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TransactionService.Data;

namespace TransactionService.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TransactionService.Models.Price", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Area")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("PricePerKM")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("Prices");
                });

            modelBuilder.Entity("TransactionService.Models.StatusOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("StatusOrders");
                });

            modelBuilder.Entity("TransactionService.Models.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("Billing")
                        .HasColumnType("real");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("DriverId")
                        .HasColumnType("int");

                    b.Property<double>("LatEnd")
                        .HasColumnType("float");

                    b.Property<double>("LatStart")
                        .HasColumnType("float");

                    b.Property<double>("LongEnd")
                        .HasColumnType("float");

                    b.Property<double>("LongStart")
                        .HasColumnType("float");

                    b.Property<int>("PriceId")
                        .HasColumnType("int");

                    b.Property<int>("StatusOrderId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PriceId");

                    b.HasIndex("StatusOrderId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("TransactionService.Models.WalletMutation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<float>("Credit")
                        .HasColumnType("real");

                    b.Property<float>("Debit")
                        .HasColumnType("real");

                    b.Property<float>("Saldo")
                        .HasColumnType("real");

                    b.Property<int>("WalletUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WalletUserId");

                    b.ToTable("WalletMutations");
                });

            modelBuilder.Entity("TransactionService.Models.WalletUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("Rolename")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("WalletUsers");
                });

            modelBuilder.Entity("TransactionService.Models.Transaction", b =>
                {
                    b.HasOne("TransactionService.Models.Price", "Price")
                        .WithMany()
                        .HasForeignKey("PriceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TransactionService.Models.StatusOrder", "StatusOrder")
                        .WithMany()
                        .HasForeignKey("StatusOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Price");

                    b.Navigation("StatusOrder");
                });

            modelBuilder.Entity("TransactionService.Models.WalletMutation", b =>
                {
                    b.HasOne("TransactionService.Models.WalletUser", "WalletUser")
                        .WithMany("WalletMutations")
                        .HasForeignKey("WalletUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WalletUser");
                });

            modelBuilder.Entity("TransactionService.Models.WalletUser", b =>
                {
                    b.Navigation("WalletMutations");
                });
#pragma warning restore 612, 618
        }
    }
}
