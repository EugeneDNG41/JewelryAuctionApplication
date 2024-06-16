using JewelryAuctionDemo.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionDemo.Configuration;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Post");

        builder.HasKey(p => p.PostId);

        builder.Property(p => p.Title)
               .IsRequired();

        builder.Property(p => p.PostCategory)
               .IsRequired()
               .HasConversion<int>(); // Maps the enum to an integer column

        builder.Property(p => p.PostDate)
               .IsRequired();

        builder.Property(p => p.Body);

        builder.Property(p => p.Status)
               .IsRequired();

        builder.Property(p => p.Image);

        builder.HasOne(p => p.Author)
               .WithMany(a => a.Posts)
               .HasForeignKey(p => p.AccountId)
               .IsRequired();
    }
}
