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
    public Auction? LatestAuction { get; }
    public BitmapImage DisplayedImage => ByteArrayToBitmapImage(Jewelry.Image);
    
    public string TimeLeft => ComputeTimeLeft();
    public string BidNumber => ComputeBidNumber();
    public string Winner => LatestAuction?.Account != null ? LatestAuction.Account.Username : "No Winner";
    public int AuctionNumber => Jewelry.Auctions.Count;
    public ICommand NavigateJewelryPageCommand { get; }
    

    public JewelryListingViewModel(Jewelry jewelry, Auction? latestAuction,
        ParameterNavigationService<JewelryListingViewModel, JewelryPageViewModel> navigateJewelryPageService)
    {
        Jewelry = jewelry;
        LatestAuction = latestAuction;
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
        if (LatestAuction != null && LatestAuction.Bids.Any())
        {
            var bids = LatestAuction.Bids.OrderByDescending(b => b.BidAmount);
            foreach (var bid in bids)
            {
                if ((bid.Account == LatestAuction.Account) || bid.Account.Status)
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
        if (LatestAuction != null && LatestAuction.EndDate < DateTime.Now)
        {
            return "Ended";
        }
        else if (LatestAuction != null && LatestAuction.EndDate > DateTime.Now)
        {
            TimeSpan timeDifference = LatestAuction.EndDate.Subtract(DateTime.Now);
            return $"Ends in {timeDifference.Days}d {timeDifference.Hours}h {timeDifference.Minutes}m";
        } else
        {
            return "No Auction Yet";
        }
    }

    private string ComputeBidNumber()
    {
        if (LatestAuction == null)
        {
            return "No Auction Yet";
        }
        else
        {
            int bidCount = LatestAuction.Bids.Count(b => b.Account.Status);
            return bidCount > 1 ? $"{bidCount} bids" : $"{bidCount} bid";
        }
    }
}
