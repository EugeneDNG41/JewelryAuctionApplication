using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JewelryAuctionDemo.Configuration;
using JewelryAuctionDemo.Models;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Configuration;



namespace JewelryAuctionDemo;

public class JewelryAuctionContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Auction> Auctions { get; set; }
    public DbSet<Bid> Bids { get; set; }
    public DbSet<Jewelry> Jewelries { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Request> Requests { get; set; }
    public JewelryAuctionContext(DbContextOptions<JewelryAuctionContext> options) :base(options) { }
    public JewelryAuctionContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            IConfiguration configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("JewelryAuctionDatabase"));
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AccountConfiguration());
        modelBuilder.ApplyConfiguration(new AuctionConfiguration());
        modelBuilder.ApplyConfiguration(new BidConfiguration());
        modelBuilder.ApplyConfiguration(new JewelryConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentConfiguration());
        modelBuilder.ApplyConfiguration(new PostConfiguration());
        modelBuilder.ApplyConfiguration(new RequestConfiguration());
        // Apply other configurations if necessary
        modelBuilder.Entity<Account>().HasData(
        new Account
        {
            AccountId = 1,
            Username = "admin",
            Password = "adminpassword", // Ensure you hash passwords in a real application
            FullName = "Admin User",
            Email = "admin@example.com",
            Birthday = new DateTime(1990, 1, 1),
            PhoneNumber = "1234567890",
            Status = true,
            Role = Role.ADMIN
        }
    );
    }

}
