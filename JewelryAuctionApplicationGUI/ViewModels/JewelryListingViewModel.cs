using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Commands;
using JewelryAuctionApplicationGUI.Navigation;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace JewelryAuctionApplicationGUI.ViewModels;

public class JewelryListingViewModel : BaseViewModel
{
    public Jewelry Jewelry { get; }
    public Auction LatestAuction { get; private set; }
    public BitmapImage DisplayedImage => ByteArrayToBitmapImage(Jewelry.Image);
    
    public string TimeLeft => ComputeTimeLeft();
    public string BidNumber => ComputeBidNumber();
    public ICommand NavigateJewelryPageCommand { get; }
    public IAuctionService AuctionService { get; }
    private readonly IBidService _bidService;
    private readonly IJewelryService _jewelryService;

    public JewelryListingViewModel(Jewelry jewelry, Auction auction,
        ParameterNavigationService<JewelryListingViewModel, JewelryPageViewModel> navigateJewelryPageService,
        IAuctionService auctionService, IBidService bidService, IJewelryService jewelryService)
    {
        Jewelry = jewelry;
        LatestAuction = auction;
        AuctionService = auctionService;
        _bidService = bidService;
        _jewelryService = jewelryService;
        NavigateJewelryPageCommand = new NavigateJewelryPageCommand(this, navigateJewelryPageService);
        UpdateCurrentPrice();
    }

    private BitmapImage ByteArrayToBitmapImage(byte[] byteArray)
    {
        using (MemoryStream stream = new MemoryStream(byteArray))
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }
    }
    public void UpdateCurrentPrice()
    { 
        if (LatestAuction.Bids.Any())
        {
            var bids = LatestAuction.Bids.OrderByDescending(b => b.BidAmount);
            foreach (var bid in bids)
            {
                if (bid.Account.Status)
                {
                    LatestAuction.CurrentPrice = bid.BidAmount;
                    break;
                }
                LatestAuction.CurrentPrice = LatestAuction.Jewelry.StartingPrice;
            }
            OnPropertyChanged(nameof(LatestAuction));
        }
    }
    private string ComputeTimeLeft()
    {
        if (LatestAuction == null || LatestAuction.EndDate < DateTime.Now)
        {
            return "Ended";
        }
        else
        {
            TimeSpan timeDifference = LatestAuction.EndDate.Subtract(DateTime.Now);
            return $"Ends in {timeDifference.Days}d {timeDifference.Hours}h {timeDifference.Minutes}m";
        }
    }

    private string ComputeBidNumber()
    {
        if (LatestAuction == null || LatestAuction.Bids == null)
        {
            return "0 bids";
        }
        else
        {
            int bidCount = LatestAuction.Bids.Count(b => b.Account.Status);
            return bidCount > 1 ? $"{bidCount} bids" : $"{bidCount} bid";
        }
    }
}
