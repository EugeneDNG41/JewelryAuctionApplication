﻿using JewelryAuctionDemo.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionDemo.Configuration;

public class JewelryConfiguration : IEntityTypeConfiguration<Jewelry>
{
    public void Configure(EntityTypeBuilder<Jewelry> builder)
    {
        builder.ToTable("Jewelry");

        builder.HasKey(j => j.JewelryId);

        builder.Property(j => j.JewelryName)
               .IsRequired();

        builder.Property(j => j.Description);

        builder.Property(j => j.JewelryCategory)
               .IsRequired()
               .HasConversion<int>(); // Maps the enum to an integer column

        builder.Property(j => j.Condition)
               .IsRequired();

        builder.Property(j => j.Estimate)
               .IsRequired();

        builder.Property(j => j.StartingPrice)
               .IsRequired()
               .HasColumnType("decimal(18,2)"); // Specifies the decimal precision and scale

        builder.Property(j => j.Status)
               .IsRequired();

        builder.HasMany(j => j.Auctions)
               .WithOne(a => a.Jewelry)
               .HasForeignKey(a => a.JewelryId);

        builder.HasMany(j => j.Requests)
               .WithOne(r => r.Jewelry)
               .HasForeignKey(r => r.JewelryId);
    }
}
