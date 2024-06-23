
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplicationDAL;

public class JewelryAuctionContextFactory : IDesignTimeDbContextFactory<JewelryAuctionContext>
{
    public JewelryAuctionContext CreateDbContext(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        IConfiguration configuration = builder.Build();
        var optionsBuilder = new DbContextOptionsBuilder<JewelryAuctionContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("JewelryAuctionDatabase"));
        return new JewelryAuctionContext(optionsBuilder.Options);
    }
}
