﻿using JewelryAuctionDemo.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionDemo.Configuration;

public class AuctionConfiguration : IEntityTypeConfiguration<Auction>
{
    public void Configure(EntityTypeBuilder<Auction> builder)
    {
        builder.ToTable("Auction");

        builder.HasKey(a => a.AuctionId);

        builder.Property(a => a.StartDate)
               .IsRequired();

        builder.Property(a => a.EndDate)
               .IsRequired();

        builder.Property(a => a.CurrentPrice)
               .IsRequired()
               .HasColumnType("decimal(18,2)") // Specifies the decimal precision and scale
               .HasDefaultValue(0);

        builder.Property(a => a.Status)
               .IsRequired();

        builder.HasOne(a => a.Jewelry)
               .WithMany(j => j.Auctions)
               .HasForeignKey(a => a.JewelryId)
               .IsRequired();

        builder.HasMany(a => a.Bids)
               .WithOne(b => b.Auction)
               .HasForeignKey(b => b.AuctionId);

        builder.HasMany(a => a.Payments)
               .WithOne(p => p.Auction)
               .HasForeignKey(p => p.AuctionId);
    }
}
