using JewelryAuctionApplicationDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace JewelryAuctionApplicationDAL.Configuration;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        var dateOnlyConverter = new ValueConverter<DateOnly, DateTime>(
                v => v.ToDateTime(TimeOnly.MinValue),
                v => DateOnly.FromDateTime(v)
            );
        builder.ToTable("Account");

        builder.HasKey(a => a.AccountId);

        builder.Property(a => a.AccountId)
               .ValueGeneratedOnAdd();

        builder.Property(a => a.Username)
               .IsRequired();

        builder.Property(a => a.Password)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(a => a.FullName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(a => a.Email)
               .IsRequired()
               .HasMaxLength(100)
               .HasAnnotation("EmailAddress", true);

        builder.Property(a => a.Birthday)
                .HasConversion(dateOnlyConverter)
                .HasColumnType("Date");

        /*builder.Property(a => a.PhoneNumber)
               .HasMaxLength(12);
               //.HasAnnotation("RegularExpression", @"^[0-9]{8,12}$");*/

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
    }
}
