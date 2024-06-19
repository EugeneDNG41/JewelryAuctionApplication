using JewelryAuctionApplication.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplication.Configuration;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payment");

        builder.HasKey(p => p.PaymentId);

        builder.Property(p => p.PaymentId)
               .ValueGeneratedOnAdd();

        builder.Property(p => p.PaymentMethod)
               .IsRequired();

        builder.Property(p => p.Subtotal)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(p => p.Tax)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(p => p.Shipping)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(p => p.Total)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(p => p.Status)
               .IsRequired()
               .HasConversion<int>(); // Maps the enum to an integer column

        builder.HasOne(p => p.Auction)
               .WithMany(a => a.Payments)
               .HasForeignKey(p => p.AuctionId)
               .IsRequired();

        builder.HasOne(p => p.Account)
               .WithMany(a => a.Payments)
               .HasForeignKey(p => p.AccountId)
               .IsRequired();
    }
}
