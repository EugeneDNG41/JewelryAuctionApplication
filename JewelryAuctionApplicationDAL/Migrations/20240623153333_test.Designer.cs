﻿// <auto-generated />
using System;
using JewelryAuctionApplicationDAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JewelryAuctionApplicationDAL.Migrations
{
    [DbContext(typeof(JewelryAuctionContext))]
    [Migration("20240623153333_test")]
    partial class test
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("JewelryAuctionApplicationDAL.Models.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountId"));

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("Date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasAnnotation("EmailAddress", true);

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AccountId");

                    b.ToTable("Account", (string)null);
                });

            modelBuilder.Entity("JewelryAuctionApplicationDAL.Models.Auction", b =>
                {
                    b.Property<int>("AuctionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AuctionId"));

                    b.Property<decimal>("CurrentPrice")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(18,2)")
                        .HasDefaultValue(0m);

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("JewelryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("AuctionId");

                    b.HasIndex("JewelryId");

                    b.ToTable("Auction", (string)null);
                });

            modelBuilder.Entity("JewelryAuctionApplicationDAL.Models.Bid", b =>
                {
                    b.Property<int>("BidId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BidId"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<int>("AuctionId")
                        .HasColumnType("int");

                    b.Property<decimal>("BidAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("BidTime")
                        .HasColumnType("datetime2");

                    b.HasKey("BidId");

                    b.HasIndex("AccountId");

                    b.HasIndex("AuctionId");

                    b.ToTable("Bid", (string)null);
                });

            modelBuilder.Entity("JewelryAuctionApplicationDAL.Models.Jewelry", b =>
                {
                    b.Property<int>("JewelryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("JewelryId"));

                    b.Property<string>("Condition")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Estimate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("JewelryCategory")
                        .HasColumnType("int");

                    b.Property<string>("JewelryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("StartingPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("JewelryId");

                    b.ToTable("Jewelry", (string)null);
                });

            modelBuilder.Entity("JewelryAuctionApplicationDAL.Models.Payment", b =>
                {
                    b.Property<int>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentId"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<int>("AuctionId")
                        .HasColumnType("int");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Shipping")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<decimal>("Subtotal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Tax")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("PaymentId");

                    b.HasIndex("AccountId");

                    b.HasIndex("AuctionId");

                    b.ToTable("Payment", (string)null);
                });

            modelBuilder.Entity("JewelryAuctionApplicationDAL.Models.Request", b =>
                {
                    b.Property<int>("RequestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RequestId"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<decimal>("FinalValuation")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("JewelryId")
                        .HasColumnType("int");

                    b.Property<decimal>("PreliminaryValuation")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("ValuationDate")
                        .HasColumnType("datetime2");

                    b.HasKey("RequestId");

                    b.HasIndex("AccountId");

                    b.HasIndex("JewelryId");

                    b.ToTable("Request", (string)null);
                });

            modelBuilder.Entity("JewelryAuctionApplicationDAL.Models.Auction", b =>
                {
                    b.HasOne("JewelryAuctionApplicationDAL.Models.Jewelry", "Jewelry")
                        .WithMany("Auctions")
                        .HasForeignKey("JewelryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Jewelry");
                });

            modelBuilder.Entity("JewelryAuctionApplicationDAL.Models.Bid", b =>
                {
                    b.HasOne("JewelryAuctionApplicationDAL.Models.Account", "Account")
                        .WithMany("Bids")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JewelryAuctionApplicationDAL.Models.Auction", "Auction")
                        .WithMany("Bids")
                        .HasForeignKey("AuctionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Auction");
                });

            modelBuilder.Entity("JewelryAuctionApplicationDAL.Models.Payment", b =>
                {
                    b.HasOne("JewelryAuctionApplicationDAL.Models.Account", "Account")
                        .WithMany("Payments")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JewelryAuctionApplicationDAL.Models.Auction", "Auction")
                        .WithMany("Payments")
                        .HasForeignKey("AuctionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Auction");
                });

            modelBuilder.Entity("JewelryAuctionApplicationDAL.Models.Request", b =>
                {
                    b.HasOne("JewelryAuctionApplicationDAL.Models.Account", "Account")
                        .WithMany("Requests")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JewelryAuctionApplicationDAL.Models.Jewelry", "Jewelry")
                        .WithMany("Requests")
                        .HasForeignKey("JewelryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Jewelry");
                });

            modelBuilder.Entity("JewelryAuctionApplicationDAL.Models.Account", b =>
                {
                    b.Navigation("Bids");

                    b.Navigation("Payments");

                    b.Navigation("Requests");
                });

            modelBuilder.Entity("JewelryAuctionApplicationDAL.Models.Auction", b =>
                {
                    b.Navigation("Bids");

                    b.Navigation("Payments");
                });

            modelBuilder.Entity("JewelryAuctionApplicationDAL.Models.Jewelry", b =>
                {
                    b.Navigation("Auctions");

                    b.Navigation("Requests");
                });
#pragma warning restore 612, 618
        }
    }
}
