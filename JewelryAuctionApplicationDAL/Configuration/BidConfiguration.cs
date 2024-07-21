using JewelryAuctionApplicationDAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplicationDAL.Configuration;

public class BidConfiguration : IEntityTypeConfiguration<Bid>
{
    public void Configure(EntityTypeBuilder<Bid> builder)
    {
        builder.ToTable("Bid");

        builder.HasKey(b => b.BidId);

        builder.Property(b => b.BidId)
               .ValueGeneratedOnAdd();

        builder.Property(b => b.BidAmount)
               .IsRequired()
               .HasColumnType("decimal(18)"); // Specifies the decimal precision and scale

        builder.Property(b => b.BidTime)
               .IsRequired();

        builder.HasOne(b => b.Auction)
               .WithMany(a => a.Bids)
               .HasForeignKey(b => b.AuctionId)
               .IsRequired();

        builder.HasOne(b => b.Account)
               .WithMany(a => a.Bids)
               .HasForeignKey(b => b.AccountId)
               .IsRequired();
    }
}
