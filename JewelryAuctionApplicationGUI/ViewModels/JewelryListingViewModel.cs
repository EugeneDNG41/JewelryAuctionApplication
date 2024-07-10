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
    private DispatcherTimer _auctionTimer;
    public Jewelry Jewelry { get; }
    public Auction? ActiveAuction => AuctionService.GetOngoingByJewelryId(Jewelry.JewelryId);
    public BitmapImage DisplayedImage => ByteArrayToBitmapImage(Jewelry.Image);
    private Account? _winner;
    public Account? Winner
    {
        get => _winner;
        private set
        {
            _winner = value;
            OnPropertyChanged(nameof(Winner));
        }
    }

    private string _timeLeft;
    public string TimeLeft
    {
        get => _timeLeft;
        private set
        {
            _timeLeft = value;
            OnPropertyChanged(nameof(TimeLeft));
        }
    }

    private string _bidBoxTitle;
    public string BidBoxTitle
    {
        get
        {
            int bidCount = ActiveAuction?.Bids.Count ?? 0;
            _bidBoxTitle = bidCount > 0 ? $"Current Price ({bidCount} bids)" : "Starting Price";
            return _bidBoxTitle;
        }
        set
        {
            _bidBoxTitle = value;
            OnPropertyChanged(nameof(BidBoxTitle));
        }
    }

    public bool CanBid => ActiveAuction != null && ActiveAuction.EndDate > DateTime.Now;
    public ICommand NavigateJewelryPageCommand { get; }
    public IAuctionService AuctionService { get; }
    public IBidService BidService { get; }

    public JewelryListingViewModel(Jewelry jewelry,
        ParameterNavigationService<JewelryListingViewModel, JewelryPageViewModel> navigateJewelryPageService,
        IAuctionService auctionService, IBidService bidService)
    {
        Jewelry = jewelry;
        AuctionService = auctionService;
        BidService = bidService;
        NavigateJewelryPageCommand = new NavigateJewelryPageCommand(this, navigateJewelryPageService);

        InitializeTimer();
    }

    private void InitializeTimer()
    {
        _auctionTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(100)
        };
        _auctionTimer.Tick += AuctionTimer_Tick;
        _auctionTimer.Start();
    }

    private void AuctionTimer_Tick(object sender, EventArgs e)
    {
        if (ActiveAuction != null && ActiveAuction.EndDate > DateTime.Now)
        {
            TimeSpan timeDifference = ActiveAuction.EndDate.Subtract(DateTime.Now);
            TimeLeft = $"Ends in {timeDifference.Days}d {timeDifference.Hours}h {timeDifference.Minutes}m {timeDifference.Seconds}s";
            Bid? bid = BidService.GetHighestBid(ActiveAuction.AuctionId);
            if (ActiveAuction.CurrentPrice < bid?.BidAmount)
            {
                ActiveAuction.CurrentPrice = bid.BidAmount;
                OnPropertyChanged(nameof(ActiveAuction));
                OnPropertyChanged(nameof(BidBoxTitle));
            }
        }
        else
        {
            TimeLeft = "Ended";
            _auctionTimer.Stop();
            OnPropertyChanged(nameof(CanBid));
            Winner = BidService.GetHighestBid(ActiveAuction.AuctionId)?.Account;
        }
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
}
