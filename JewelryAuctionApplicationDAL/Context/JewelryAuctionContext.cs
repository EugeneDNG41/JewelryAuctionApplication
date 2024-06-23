using JewelryAuctionApplicationDAL.Configuration;
using JewelryAuctionApplicationDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace JewelryAuctionApplicationDAL.Context;

public class JewelryAuctionContext : DbContext
{
    public virtual DbSet<Account> Accounts { get; set; }
    public virtual DbSet<Auction> Auctions { get; set; }
    public virtual DbSet<Bid> Bids { get; set; }
    public virtual DbSet<Jewelry> Jewelries { get; set; }
    public virtual DbSet<Payment> Payments { get; set; }
    public virtual DbSet<Request> Requests { get; set; }
    public JewelryAuctionContext(DbContextOptions<JewelryAuctionContext> options) : base(options) { }
    public JewelryAuctionContext() { }

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
        modelBuilder.ApplyConfiguration(new RequestConfiguration());
        // Apply other configurations if necessary      
    }
}
