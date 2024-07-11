using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Commands;
using JewelryAuctionApplicationGUI.Navigation;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace JewelryAuctionApplicationGUI.ViewModels;

public class JewelryListingViewModel : BaseViewModel
{
    public Jewelry Jewelry { get; }
    public Auction? LatestAuction => AuctionService.GetLatestByJewelryId(Jewelry.JewelryId);
    public BitmapImage DisplayedImage => ByteArrayToBitmapImage(Jewelry.Image);
    
    public string TimeLeft
    {
        get
        {
            if (LatestAuction.EndDate < DateTime.Now)
            {
                return "Ended";
            } else
            {
                TimeSpan timeDifference = LatestAuction.EndDate.Subtract(DateTime.Now);
                return $"Ends in {timeDifference.Days}d {timeDifference.Hours}h {timeDifference.Minutes}m";
            }
            
        }

    }
    private string _bidNumber;
    public string BidNumber
    {
        get
        {
            int bidCount = LatestAuction?.Bids.Count ?? 0;
            _bidNumber = bidCount > 1 ? $"{bidCount} bids" : $"{bidCount} bid";
            return _bidNumber;
        }
        set
        {
            _bidNumber = value;
            OnPropertyChanged(nameof(BidNumber));
        }
    }
    public ICommand NavigateJewelryPageCommand { get; }
    public IAuctionService AuctionService { get; }
    public IBidService BidService { get; }
    public IJewelryService JewelryService { get; }

    public JewelryListingViewModel(Jewelry jewelry,
        ParameterNavigationService<JewelryListingViewModel, JewelryPageViewModel> navigateJewelryPageService,
        IAuctionService auctionService, IBidService bidService, IJewelryService jewelryService)
    {
        Jewelry = jewelry;
        AuctionService = auctionService;
        BidService = bidService;
        JewelryService = jewelryService;
        NavigateJewelryPageCommand = new NavigateJewelryPageCommand(this, navigateJewelryPageService);
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
        if (LatestAuction != null)
        {
            var highestBid = BidService.GetHighestBid(LatestAuction.AuctionId);
            LatestAuction.CurrentPrice = highestBid != null ? highestBid.BidAmount : LatestAuction.CurrentPrice;
            OnPropertyChanged(nameof(LatestAuction));
        }
    }
}
