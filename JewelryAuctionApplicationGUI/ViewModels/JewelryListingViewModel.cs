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

namespace JewelryAuctionApplicationGUI.ViewModels;

public class JewelryListingViewModel : BaseViewModel
{
    public Jewelry Jewelry {  get; }
    private Auction? _activeAuction;
    public Auction? ActiveAuction
    {
        get
        {
            return AuctionService.GetOngoingByJewelryId(Jewelry.JewelryId);
        }
        set
        {
            _activeAuction = value;
            UpdateListing();
            OnPropertyChanged(nameof(ActiveAuction));
            OnPropertyChanged(nameof(TimeLeft));
            OnPropertyChanged(nameof(BidBoxTitle));
            OnPropertyChanged(nameof(CanBid));
        }
    }
    public BitmapImage DisplayedImage => ByteArrayToBitmapImage(Jewelry.Image);
    private string _timeLeft;
    public string TimeLeft
    {
        get
        {
            if (ActiveAuction != null && ActiveAuction.EndDate > DateTime.Now)
            {
                TimeSpan TimeDifference = ActiveAuction.EndDate.Subtract(DateTime.Now);
                _timeLeft = $"Ends in {TimeDifference.Days}d {TimeDifference.Hours}h {TimeDifference.Minutes}m";
            }
            else
            {
                _timeLeft = "Ended";
            }
            return _timeLeft;
        }
    }
    private string _bidBoxTitle;
    public string BidBoxTitle
    {
        get
        {
            int bidCount = ActiveAuction?.Bids.Count ?? 0;
            if (bidCount > 0)
            {
                _bidBoxTitle = $"Current Price ({bidCount} bids)";
            }
            else
            {
                _bidBoxTitle = "Starting Price";
            }
            return _bidBoxTitle;
        }
    }
    public bool CanBid => ActiveAuction != null && ActiveAuction.EndDate > DateTime.Now;
    public ICommand NavigateJewelryPageCommand { get; }
    public IAuctionService AuctionService { get; }
    public IBidService BidService { get; }
    public JewelryListingViewModel(Jewelry jewelry,
        ParameterNavigationService<JewelryListingViewModel, JewelryPageViewModel> navigateJewelryPageService,
        IAuctionService auctionService, IBidService bidService
        )
    {
        Jewelry = jewelry;
        AuctionService = auctionService;
        BidService = bidService;
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
    public void UpdateListing()
    {
        if (ActiveAuction != null)
        {
            ActiveAuction.CurrentPrice = ActiveAuction.Bids.Max(b => b.BidAmount);
            OnPropertyChanged(nameof(ActiveAuction));         
        }
    }
}
