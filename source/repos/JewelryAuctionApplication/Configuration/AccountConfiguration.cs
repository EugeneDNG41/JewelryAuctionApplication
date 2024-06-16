using JewelryAuctionDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionDemo.Configuration;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Account");

        builder.HasKey(a => a.AccountId);

        builder.Property(a => a.Username)
               .IsRequired();

        builder.Property(a => a.Password)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(a => a.FullName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(a => a.Email)
               .IsRequired()
               .HasMaxLength(100)
               .HasAnnotation("EmailAddress", true);

        builder.Property(a => a.Image);

        builder.Property(a => a.Birthday)
               .IsRequired();

        builder.Property(a => a.PhoneNumber)
               .IsRequired()
               .HasMaxLength(12)
               .HasAnnotation("RegularExpression", @"^[0-9]{8,12}$");

        builder.Property(a => a.Status)
               .IsRequired();

        builder.Property(a => a.Role)
               .IsRequired()
               .HasConversion<int>(); // Maps the enum to an integer column

        builder.HasMany(a => a.Bids)
               .WithOne()
               .HasForeignKey(b => b.AccountId);

        builder.HasMany(a => a.Requests)
               .WithOne()
               .HasForeignKey(r => r.AccountId);

        builder.HasMany(a => a.Payments)
               .WithOne()
               .HasForeignKey(p => p.AccountId);

        builder.HasMany(a => a.Posts)
               .WithOne()
               .HasForeignKey(p => p.AccountId);
    }
}
