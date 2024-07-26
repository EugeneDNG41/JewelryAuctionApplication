using JewelryAuctionApplicationDAL.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplicationBLL.Services;

public class AuctionCheckService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public AuctionCheckService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await CheckAuctionsAsync();
            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken); // Adjust the interval as needed
        }
    }

    private async Task CheckAuctionsAsync()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var auctionService = scope.ServiceProvider.GetRequiredService<IAuctionService>();
            var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
            var jewelryService = scope.ServiceProvider.GetRequiredService<IJewelryService>();

            var auctions = await auctionService.GetAllLatestAsync();
            foreach (var auction in auctions)
            {
                if (auction.EndDate < DateTime.Now)
                {
                    var jewelry = auction.Jewelry;
                    if (jewelry != null)
                    {
                        if (auction.Bids.Any())
                        {
                            var bids = auction.Bids.OrderByDescending(b => b.BidAmount).ToList();
                            var winner = bids.FirstOrDefault(b => b.Account.Status)?.Account;
                            auction.CurrentPrice = winner != null ? bids.First(b => b.Account.Status).BidAmount : jewelry.StartingPrice;

                            if (auction.CurrentPrice == jewelry.StartingPrice)
                            {
                                jewelry.Status = JewelryStatus.READY;
                            }
                            else
                            {
                                jewelry.Status = JewelryStatus.SOLD;
                                auction.Account = winner;
                                winner.Credit -= auction.CurrentPrice;
                                await accountService.UpdateAsync(winner);
                            }
                            await auctionService.UpdateAsync(auction);
                        }
                        else
                        {
                            jewelry.Status = JewelryStatus.READY;
                        }
                        await jewelryService.UpdateAsync(jewelry);
                    }
                }
            }
        }
    }
}
