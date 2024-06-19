using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JewelryAuctionApplication.Models;

namespace JewelryAuctionApplication.Configuration;

public class RequestConfiguration : IEntityTypeConfiguration<Request>
{
    public void Configure(EntityTypeBuilder<Request> builder)
    {
        builder.ToTable("Request");

        builder.HasKey(r => r.RequestId);

        builder.Property(r => r.RequestId)
               .ValueGeneratedOnAdd();

        builder.Property(r => r.RequestDate)
               .IsRequired();

        builder.Property(r => r.ValuationDate)
               .IsRequired();

        builder.Property(r => r.PreliminaryValuation)
               .HasColumnType("decimal(18,2)");

        builder.Property(r => r.FinalValuation)
               .HasColumnType("decimal(18,2)");

        builder.Property(r => r.Status)
               .IsRequired();

        builder.HasOne(r => r.Jewelry)
               .WithMany(j => j.Requests)
               .HasForeignKey(r => r.JewelryId)
               .IsRequired();

        builder.HasOne(r => r.Account)
               .WithMany(a => a.Requests)
               .HasForeignKey(r => r.AccountId)
               .IsRequired();
    }
}
