using JewelryAuctionApplicationDAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplicationDAL.Configuration;

public class AuctionConfiguration : IEntityTypeConfiguration<Auction>
{
    public void Configure(EntityTypeBuilder<Auction> builder)
    {
        builder.ToTable("Auction");

        builder.HasKey(a => a.AuctionId);

        builder.Property(a => a.AuctionId)
               .ValueGeneratedOnAdd();

        builder.Property(a => a.EndDate)
               .IsRequired();

        builder.Property(a => a.CurrentPrice)
               .IsRequired()
               .HasColumnType("decimal(18)") // Specifies the decimal precision and scale
               .HasDefaultValue(0);

        builder.HasOne(a => a.Jewelry)
               .WithMany(j => j.Auctions)
               .HasForeignKey(a => a.JewelryId)
               .IsRequired();
        builder.HasOne(a => a.Account)
               .WithMany(ac => ac.Auctions)
               .HasForeignKey(a => a.AccountId);

        builder.HasMany(a => a.Bids)
               .WithOne(b => b.Auction)
               .HasForeignKey(b => b.AuctionId);
    }
}
