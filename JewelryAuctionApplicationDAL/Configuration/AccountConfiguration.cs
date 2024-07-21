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

        builder.Property(a => a.Status)
               .IsRequired();

        builder.Property(a => a.Credit)
               .IsRequired()
               .HasColumnType("decimal(18)");

        builder.Property(a => a.Role)
               .IsRequired()
               .HasConversion<int>(); // Maps the enum to an integer column

        builder.HasMany(a => a.Bids)
               .WithOne()
               .HasForeignKey(b => b.AccountId);
    }
}
